using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkingManger : NetworkManager
{
    [SerializeField] LoginPopUP _loginPopUp;
    [SerializeField] ChatingUI _chatingUI;

    public void OnInputValueChanged_SetHostName(string hostName)
    {
        this.networkAddress = hostName;
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)//���� ��Ŀ��Ʈ ������
    {
        if (_chatingUI != null)
        {
            //[TODO]_chatingUI.
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnClientDisconnect()//Ŭ�� ��Ŀ��Ʈ ������
    {
        base.OnClientDisconnect();

        if(_loginPopUp != null)
        {
            _loginPopUp.SetUIOnClientDisconnected();
        }
    }
}
