using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginPopUP : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField Input_NetworkAdress;
    [SerializeField]
    private TMP_InputField Input_UserName;

    [SerializeField] private Button Btn_StartAsHostServer;
    [SerializeField] private Button Btn_StartAsClient;

    [SerializeField] protected Text Text_Error;

    private string _originNetworkAdress;

    private void Awake()
    {
        
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
        
    }

    public void OnClick_StartAsHost()
    {

    }

    public void OnClick_StartAsClient()
    {

    }

    public void OnValueChanged_ToggleButton(string userName)
    {

    }


}
