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

    //���� �¸� -����� �÷��̾� �̸�
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
    [Command(requiresAuthority = false)]//Ŭ�󿡼� ������ ���𰡸� ��Ų��(��û�Ѵ�)
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
            OnRecvMessage(senderName, msg.Trim());//��ε� ĳ����-> ����� ��� Ŭ��?� ���� �̺�Ʈ�� �߻���Ŵ
        }

    }

    [ClientRpc]
    void OnRecvMessage(string senderName, string msg)
    {
        string formatedMsg = (senderName == _localPlayerName) ?
            $"<color=red>{senderName}:</color>{msg}" :
            $"<color=blue>{senderName}:</color>{msg}";//�̸��� ���� ���ٲ�


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
        //Text_ChatHistory.text += $"{msg}\n"; �ؿ����� ����
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
