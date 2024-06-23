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
    [SerializeField] Button logOutBtn;
    [SerializeField] GameObject container;
    int startsceneNumber = 0;

    HashSet<string> users;

    void Start()
    {
        users = new ();
        logOutBtn.onClick.AddListener(LogOut);
        OnServerStatusRequestHandler.OnServerStatusRequest += RefreshUI;
        RequestServerStatus();
    }

    void OnDestroy()
    {
        logOutBtn?.onClick.RemoveListener(LogOut);
        OnServerStatusRequestHandler.OnServerStatusRequest -= RefreshUI;
    }

    void RefreshUI(OnServerStatus message)
    {
        for(int i = 0; i < message.TopPlayers.Length; i++)
        {
            NetPlayerDto player = message.TopPlayers[i];
            if (users.Contains(player.Username)) continue;
            UserUI userUI = Instantiate(userUISO.UserUI, container.transform);
            userUI.Username.text = player.Username;
            userUI.Score.text = player.Score.ToString();
            userUI.SetOnlineStatus(player.IsOnline);
            users.Add(player.Username);
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
        SceneManager.LoadScene(startsceneNumber);
    }
}
