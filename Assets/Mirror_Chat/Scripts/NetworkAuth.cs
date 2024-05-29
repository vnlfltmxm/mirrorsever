using Mirror;
using Mirror.SimpleWeb;
using Org.BouncyCastle.Asn1.Cmp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkAuth : NetworkAuthenticator
{

    //서버
    readonly HashSet<NetworkConnection> _connentionPendingDisconnect = new HashSet<NetworkConnection>();
    internal static readonly HashSet<string> _playerNamse = new HashSet<string>();

    //클라
   

    public struct AuthReqMsg : NetworkMessage
    {
        //인증을 위해 사용
        // OAuth 같은걸 사용시에는 이부분에서 엑세스 토큰같은 변수를 추가하면 됨
        public string authUserName;
    }

    public struct AuthResMsg : NetworkMessage
    {
        public byte code;
        public string message;
    }

    #region SeverSide
    [UnityEngine.RuntimeInitializeOnLoadMethod]
    static void ResetStatics()
    {

    }

    public override void OnStartServer()
    {
        //클라로 부터 인증요청 처리를 위한 핸들러 연결
        NetworkServer.RegisterHandler<AuthReqMsg>(OnAuthRequestMessage, false);
    }

    public override void OnStopClient()
    {
        NetworkServer.UnregisterHandler<AuthResMsg>();
    }
    public override void OnServerAuthenticate(NetworkConnectionToClient conn)
    {

    }

    public void OnAuthRequestMessage(NetworkConnectionToClient conn,AuthReqMsg msg)
    {
        //클라인증 요청 메세지 도착시 처리
        Debug.Log($"인증 요청 : {msg.authUserName}");
        if(_connentionPendingDisconnect.Contains(conn) )
        {
            return;
        }

        //웹 서버 ,DB,Playerfab API등을 호출해 인증 확인
        if(!_playerNamse.Contains(msg.authUserName))
        {
            _playerNamse.Add(msg.authUserName);

            //대입한 인증값은 Player.OnStartServer 시점에서 읽음
            conn.authenticationData = msg.authUserName;

            AuthResMsg authResMsg = new AuthResMsg
            {
                code = 100,
                message = "Auth Success"
            };

            conn.Send(authResMsg);
            ServerAccept(conn);
        }
        else
        {
            _connentionPendingDisconnect.Add(conn);

            AuthResMsg authResMsg = new AuthResMsg
            {
                code = 200,
                message = "User name Already in Use! Try again"
            };

            conn.Send(authResMsg);
            conn.isAuthenticated = false;

            StartCoroutine(DelayDisconnect(conn, 1.0f));
        }

    }

    IEnumerator DelayDisconnect(NetworkConnectionToClient conn, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        this.ServerReject(conn);
        yield return null;
        _connentionPendingDisconnect.Remove(conn);
    }

    #endregion

    
}
