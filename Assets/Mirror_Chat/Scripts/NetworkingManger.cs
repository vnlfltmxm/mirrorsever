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
    public override void OnServerDisconnect(NetworkConnectionToClient conn)//서버 디스커넥트 됬을떄
    {
        base.OnServerDisconnect(conn);
    }

    public override void OnClientDisconnect()//클라 디스커넥트 됬을때
    {
    }
}
