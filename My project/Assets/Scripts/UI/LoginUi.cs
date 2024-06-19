using Cysharp.Threading.Tasks;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] GameObject loading;

    string username;
    string password;
    bool userNameValidate;
    bool passwordValidate;

    void Start()
    {
        usernameInput.onValueChanged.AddListener(UpdateUsername);
        passwordInput.onValueChanged.AddListener(UpdatePassword);
        loginButton.onClick.AddListener(Login);
    }

    private void OnDestroy() => loginButton.onClick.RemoveListener(Login);


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

    void ValidateAndUpdateUI() => loginButton.interactable = userNameValidate && passwordValidate;

    void Login()
    {
        loginButton.interactable = false;
        loading.SetActive(true);
        client.Connect();

        LoginAsync().Forget();
    }

    async UniTask LoginAsync()
    {
        await UniTask.Yield();
    }

    bool InputValidate(GameObject error, string input, int lengt, bool regex)
    {
        bool validate = (!string.IsNullOrWhiteSpace(input) && input.Length <= lengt) && regex;
        error.SetActive(!validate);
        return validate;
    }
}
