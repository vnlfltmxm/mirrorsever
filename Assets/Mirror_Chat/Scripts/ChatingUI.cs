using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class ChatingUI : NetworkBehaviour
{
    [SerializeField]
    Text Text_ChatHistory;
    [SerializeField]
    Scrollbar Scrollbar_Chat;
    [SerializeField]
    TMP_InputField Input_ChatMsg;
    [SerializeField]
    Button Btn_Send;

    internal static string _localPlayerName;

    //서버 온리 -연결된 플레이어 이름
    internal static readonly Dictionary<NetworkConnectionToClient, string> _connectNameDic = new Dictionary<NetworkConnectionToClient, string>();
    
    
    public void SetLocalPlayername(string userName)
    {
        _localPlayerName = userName;
        
    }

    public override void OnStartServer()
    {
        this.gameObject.SetActive(true);
        _connectNameDic.Clear();
    }

    public override void OnStartClient()
    {
        this.gameObject.SetActive(true);
        Text_ChatHistory.text = string.Empty;

    }
    [Command(requiresAuthority = false)]//클라에서 서버에 무언가를 시킨다(요청한다)
    void CommandSendMsg(string msg,NetworkConnectionToClient sender = null)
    {
        if(!_connectNameDic.ContainsKey(sender))
        {
            var player = sender.identity.GetComponent<ChatUser>();
            var playerName = player.PlayerName;
            _connectNameDic.Add(sender, playerName);
        }

        if(!string.IsNullOrWhiteSpace(msg))
        {
            var senderName = _connectNameDic[sender];
            OnRecvMessage(senderName, msg.Trim());//브로드 캐스팅-> 연결된 모든 클라?등에 전부 이벤트를 발생시킴
        }

    }

    [ClientRpc]
    void OnRecvMessage(string senderName, string msg)
    {
        string formatedMsg = (senderName == _localPlayerName) ?
            $"<color=red>{senderName}:</color>{msg}" :
            $"<color=blue>{senderName}:</color>{msg}";//이름에 따른 색바꿈


        AppendMessage(formatedMsg);

    }

    public void RemoveNameOnServerDisconnect(NetworkConnectionToClient conn)
    {
        _connectNameDic.Remove(conn);
    }

    //==============UI===============

    void AppendMessage(string msg)
    {
        StartCoroutine(AppendAndScroll(msg));
    }

    IEnumerator AppendAndScroll(string msg)
    {
        //Text_ChatHistory.text += $"{msg}\n"; 밑에꺼랑 같음
        Text_ChatHistory.text += msg + "\n";

        yield return null;
        yield return null;

        Scrollbar_Chat.value = 0;
    }


    //===============================



    public void OnClick_SendMsg()
    {
        string currentChatMsg = Input_ChatMsg.text;
        if(!string.IsNullOrWhiteSpace(currentChatMsg))
        {
            CommandSendMsg(currentChatMsg.Trim());
        }
    }

    public void OnClick_Exit()
    {
        NetworkManager.singleton.StopHost();
    }

    public void OnValueChanged_ToogleButton(string input)
    {
        Btn_Send.interactable = !string.IsNullOrWhiteSpace(input);
    }

    public void OnEndEdit_Sending()
    {
        if (Input.GetKeyDown(KeyCode.Return)
           || Input.GetKeyDown(KeyCode.KeypadEnter)
           || Input.GetKeyDown("Submit"))
        {
            OnClick_SendMsg();
        }
    }
}
