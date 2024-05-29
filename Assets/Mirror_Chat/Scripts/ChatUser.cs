using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ChatUser : NetworkBehaviour
{
    //SyncVar - ���������� ��� Ŭ�� �ڵ� ����ȭ �ϴµ� ���
    //Ŭ�� ���� �����ϸ� �ȵǰ�, �������� �����ؾ���

    [SyncVar]
    public string PlayerName;

    //ȣ��Ʈ �Ǵ� �������� ȣ��
    public override void OnStartServer()
    {
        PlayerName = (string)connectionToClient.authenticationData;
    }

    public override void OnStartLocalPlayer()//�Ѹ��� �÷��̾ �� ������?
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
