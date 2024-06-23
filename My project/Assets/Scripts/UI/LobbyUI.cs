using NetworkShared;
using TMPro;
using UnityEngine;
using Zenject;

public class LobbyUI : MonoBehaviour
{
    [Inject] NetworkingClient client;
    [SerializeField] UserUISO userUISO;
    [SerializeField] TextMeshProUGUI usersCount;
    [SerializeField] GameObject container;

    void Start()
    {
        OnServerStatusRequestHandler.OnServerStatusRequest += RefreshUI;
        RequestServerStatus();
    }

    void OnDestroy()
    {
        OnServerStatusRequestHandler.OnServerStatusRequest -= RefreshUI;
    }

    void RefreshUI(OnServerStatus message)
    {
        for(int i = 0; i < message.TopPlayers.Length; i++)
        {
            NetPlayerDto player = message.TopPlayers[i];
            UserUI userUI = Instantiate(userUISO.UserUI, container.transform);
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
}
