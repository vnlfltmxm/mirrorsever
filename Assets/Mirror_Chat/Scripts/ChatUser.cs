using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ChatUser : NetworkBehaviour
{
    //SyncVar - 서버변수를 모든 클라에 자동 동기화 하는데 사용
    //클라가 직접 변경하면 안되고, 서버에서 변경해야함

    [SyncVar]
    public string PlayerName;

    //호스트 또는 서버에만 호출
    public override void OnStartServer()
    {
        PlayerName = (string)connectionToClient.authenticationData;
    }

    public override void OnStartLocalPlayer()//한명의 플레이어만 찍어서 보낸다?
    {
        var objChatUI = GameObject.Find("ChatingUI");
        if(objChatUI != null)
        {
            var chatingUI = objChatUI.GetComponent<ChatingUI>();
            if(chatingUI != null)
            {
                chatingUI.SetLocalPlayername(PlayerName);
            }
        }
    }

}
