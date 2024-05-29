using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using static NetworkAuth;

public partial class NetworkAuth 
{
    [SerializeField] LoginPopUP _loginPopUP;

    [Header("ClientUserName")]
    public string _userName;

    #region ClientSide

    public void OnInputValueChanged_SetPlayername(string username)
    {
        _userName = username;
        _loginPopUP.SetUIOnAuthValueChanged();
    }

    public override void OnStartClient()
    {
        NetworkClient.RegisterHandler<AuthResMsg>(OnAuthResponseMessage, false);
    }

    public override void OnStopServer()
    {
        NetworkClient.UnregisterHandler<AuthResMsg>();
    }
    //Ŭ�󿡼� ���� ��û�� �ҷ���
    public override void OnClientAuthenticate()
    {
        NetworkClient.Send(new AuthReqMsg { authUserName = _userName });
    }


    //Ŭ�󿡼� ���� ����� �ҷ���
    public void OnAuthResponseMessage(AuthResMsg msg)
    {
        if (msg.code == 100)
        {
            Debug.Log($"Auth Response: {msg.code}  {msg.message}");
            ClientAccept();
        }
        else//������Ȳ
        {
            Debug.Log($"Auth Response: {msg.code}  {msg.message}");
            NetworkManager.singleton.StopHost();

            _loginPopUP.SetUIOnAuthError(msg.message);
        }
    }
    #endregion
}
