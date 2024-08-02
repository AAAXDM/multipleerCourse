using Cysharp.Threading.Tasks;
using NetworkShared;
using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoginUi : MonoBehaviour
{
    [Inject] NetworkingClient client;
    [Inject] GameManager gameManager;
    [SerializeField] int maxUsernameLenght;
    [SerializeField] int maxPasswordLenght;
    [SerializeField] ButtonText loginButton;
    [SerializeField] ButtonText registerButton;
    [SerializeField] ButtonText actionButton;
    [SerializeField] Button backButton;
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] GameObject userNameError;
    [SerializeField] GameObject passwordError;
    [SerializeField] GameObject loading;
    [SerializeField] GameObject inputObject;
    [SerializeField] GameObject startObject;
    [SerializeField] GameObject loginError;

    string username;
    string password;
    bool userNameValidate;
    bool passwordValidate;
    bool isConnected;
    bool isLogin;

    void Start()
    {
        usernameInput.onValueChanged.AddListener(UpdateUsername);
        passwordInput.onValueChanged.AddListener(UpdatePassword);
        backButton.onClick.AddListener(GoBack);
        actionButton.Button.onClick.AddListener(Login);
        loginButton.Button.onClick.AddListener(ActivateLoginWindow);
        registerButton.Button.onClick.AddListener(ActivateRegisterWindow);
        client.OnServerConnected += Connect;

        OnAuthFailedHandler.OnAuthFailed += ShowLoginError;
    }

    void OnDestroy()
    { 
        usernameInput.onValueChanged.RemoveListener(UpdateUsername);
        passwordInput.onValueChanged.RemoveListener(UpdatePassword);
        loginButton.Button.onClick.RemoveListener(ActivateLoginWindow);
        registerButton.Button.onClick.RemoveListener(ActivateRegisterWindow);
        actionButton.Button.onClick.RemoveListener(Login);
        client.OnServerConnected -= Connect;
        OnAuthFailedHandler.OnAuthFailed -= ShowLoginError;
    }


    void UpdateUsername(string value)
    {
        username = value;
        var userNameRegex = Regex.Match(value, "^[a-zA-Z0-9]+$");
        userNameValidate = InputValidate(userNameError,username,maxUsernameLenght,userNameRegex.Success);
        ValidateAndUpdateUI();
    }

    void UpdatePassword(string value)
    {
        password = value;
        passwordValidate = InputValidate(passwordError,password,maxPasswordLenght,true);
        ValidateAndUpdateUI();
    }

    void ValidateAndUpdateUI() => actionButton.Button.interactable = userNameValidate && passwordValidate;

    void Login()
    {
        actionButton.Button.interactable = false;
        loading.SetActive(true);
        client.Connect();

        LoginAsync().Forget();
    }

    void Connect() => isConnected = true;

    void ActivateLoginWindow()
    {
        isLogin = true;
        Action(loginButton);
    }

    void ActivateRegisterWindow()
    {
        Action(registerButton);
    }

    void Action(ButtonText button)
    {
        string text = button.Text;
        actionButton.SetText(text);
        startObject.SetActive(false);
        inputObject.SetActive(true);
    }

    void ShowLoginError()
    {
        actionButton.enabled = false;
        loading.SetActive(false);
        loginError.SetActive(true);
    }

    void GoBack()
    {
        inputObject.SetActive(false);
        startObject.SetActive(true);
    }

    async UniTask LoginAsync()
    {
        while (!isConnected)
        {
            await UniTask.Yield();
        }

        AuthRequestType type;
        if (isLogin)
        {
            type = AuthRequestType.Auth;
        }
        else
        {
            type = AuthRequestType.Register;
        }

        NetAuthRequest request = new(username,password, type);
        client.SendOnServer(request);
        gameManager.SetUserName(username);
    }

    bool InputValidate(GameObject error, string input, int lengt, bool regex)
    {
        bool validate = (!string.IsNullOrWhiteSpace(input) && input.Length <= lengt) && regex;
        error.SetActive(!validate);
        return validate;
    }
}
