using Mirror;
using Mirror.SimpleWeb;
using Org.BouncyCastle.Asn1.Cmp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkAuth : NetworkAuthenticator
{

    //����
    readonly HashSet<NetworkConnection> _connentionPendingDisconnect = new HashSet<NetworkConnection>();
    internal static readonly HashSet<string> _playerNamse = new HashSet<string>();

    //Ŭ��
   

    public struct AuthReqMsg : NetworkMessage
    {
        //������ ���� ���
        // OAuth ������ ���ÿ��� �̺κп��� ������ ��ū���� ������ �߰��ϸ� ��
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
        //Ŭ��� ���� ������û ó���� ���� �ڵ鷯 ����
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
        //Ŭ������ ��û �޼��� ������ ó��
        Debug.Log($"���� ��û : {msg.authUserName}");
        if(_connentionPendingDisconnect.Contains(conn) )
        {
            return;
        }

        //�� ���� ,DB,Playerfab API���� ȣ���� ���� Ȯ��
        if(!_playerNamse.Contains(msg.authUserName))
        {
            _playerNamse.Add(msg.authUserName);

            //������ �������� Player.OnStartServer �������� ����
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
