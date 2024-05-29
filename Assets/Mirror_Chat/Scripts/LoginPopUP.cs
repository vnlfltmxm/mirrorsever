using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class LoginPopUP : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField Input_NetworkAdress;
    [SerializeField]
    private TMP_InputField Input_UserName;

    [SerializeField] 
    private Button Btn_StartAsHostServer;
    [SerializeField] 
    private Button Btn_StartAsClient;

    [SerializeField] 
    private Text Text_Error;

    [SerializeField] 
    private NetworkingManger _netManger;

    private string _originNetworkAdress;

    private void Awake()
    {
        SetDefaultNetworkAddress();
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }


    private void Update()
    {
        CheckNetworkAdressValidOnUpdate();
    }

    
    private void SetDefaultNetworkAddress()
    {
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = "localhost";
        }
        _originNetworkAdress = NetworkManager.singleton.networkAddress;
    }
    private void CheckNetworkAdressValidOnUpdate()
    {
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = _originNetworkAdress;
        }

        if (Input_NetworkAdress.text != NetworkManager.singleton.networkAddress)
        {
            Input_NetworkAdress.text = NetworkManager.singleton.networkAddress;
        }
    }

    public void SetUIOnClientDisconnected()
    {
        LoginPopupSelfOpenClose(true);
        Input_UserName.text = string.Empty;
        Input_UserName.ActivateInputField();
    }

    public void SetUIOnAuthValueChanged()
    {
        //[TODO]
    } 

    private void LoginPopupSelfOpenClose(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void OnClick_StartAsHost()
    {
        if(_netManger == null)
        {
            return;
        }

        _netManger.StartHost();

        LoginPopupSelfOpenClose(false);
    }

    

    public void OnClick_StartAsClient()
    {
        if( _netManger == null)
        {
            return;
        }

        _netManger.StartClient();
        LoginPopupSelfOpenClose(false);

    }

    public void OnValueChanged_ToggleButton(string userName)
    {

    }


}
