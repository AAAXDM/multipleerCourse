using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Zenject;

public class LoginUi : MonoBehaviour
{
    [Inject] NetworkingClient client;
    [SerializeField] int maxUsernameLenght;
    [SerializeField] int maxPasswordLenght;
    [SerializeField] Button loginButton;
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] GameObject userNameError;
    [SerializeField] GameObject passwordError;

    string username;
    string password;
    bool userNameValidate;
    bool passwordValidate;

    void Start()
    {
        usernameInput.onValueChanged.AddListener(UpdateUsername);
        passwordInput.onValueChanged.AddListener(UpdatePassword);
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

    bool InputValidate(GameObject error,string input,int lengt,bool regex)
    {
        bool validate = (!string.IsNullOrWhiteSpace(input) && input.Length <= lengt) && regex;
        error.SetActive(!validate);
        return validate;
    }


    void ValidateAndUpdateUI() => loginButton.interactable = userNameValidate && passwordValidate;
}
