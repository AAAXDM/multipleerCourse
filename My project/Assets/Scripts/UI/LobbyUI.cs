using NetworkShared;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour
{
    [Inject] NetworkingClient client;
    [SerializeField] UserUISO userUISO;
    [SerializeField] TextMeshProUGUI usersCount;
    [SerializeField] Button findOpponentBtn;
    [SerializeField] Button logOutBtn;
    [SerializeField] Button stopButton;
    [SerializeField] GameObject loadingContainer;
    [SerializeField] GameObject container;
    int startSceneNumber = 0;

    Dictionary<string,UserUI> users;

    void Start()
    {
        OnFindFailedHandler.FindFaildEvent += FindFailedCallback;
        users = new ();
        logOutBtn.onClick.AddListener(LogOut);
        findOpponentBtn.onClick.AddListener(FindOpponent);
        stopButton.onClick.AddListener(StopFindingOpponent);
        OnServerStatusRequestHandler.OnServerStatusRequest += RefreshUI;
        RequestServerStatus();
    }

    void OnDestroy()
    {
        OnFindFailedHandler.FindFaildEvent -= FindFailedCallback;
        logOutBtn?.onClick.RemoveListener(LogOut);
        findOpponentBtn?.onClick.RemoveListener(FindOpponent);
        stopButton?.onClick.RemoveListener(StopFindingOpponent);
        OnServerStatusRequestHandler.OnServerStatusRequest -= RefreshUI;
    }

    void RefreshUI(OnServerStatus message)
    {
        for(int i = 0; i < message.TopPlayers.Length; i++)
        {
            NetPlayerDto player = message.TopPlayers[i];
            UserUI userUI;
            if (!users.ContainsKey(player.Username))
            {
               userUI = Instantiate(userUISO.UserUI, container.transform);
               users.Add(player.Username, userUI);
            }
            else
            {
                userUI = users[player.Username];
            }
            userUI.Username.text = player.Username;
            userUI.Score.text = player.Score.ToString();
            userUI.SetOnlineStatus(player.IsOnline);
        }

        usersCount.text = $"{message.PlayersCount} players online";
    }

    void RequestServerStatus()
    {
        ServerStatusRequest message = new();
        client.SendOnServer(message);
    }

    void LogOut()
    {
        client.Disconnect();
        SceneManager.LoadScene(startSceneNumber);
    }

    void FindOpponent()
    {
        loadingContainer.SetActive(true);
        FindOpponentRequest message = new();
        client.SendOnServer(message);
    }

    void StopFindingOpponent()
    {
        loadingContainer.SetActive(false);
        FindOpponentRequest message = new()
        {
            NeedToStop = true
        };
        client.SendOnServer(message);
    }

    void FindFailedCallback()
    {
        loadingContainer.SetActive(false);
    }
}
