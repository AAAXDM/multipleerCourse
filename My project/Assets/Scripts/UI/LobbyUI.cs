using NetworkShared;
using UnityEngine;
using Zenject;

public class LobbyUI : MonoBehaviour
{
    [Inject] NetworkingClient client;
    [SerializeField] UserUISO userUISO;
    [SerializeField] GameObject container;

    void Start()
    {
        OnServerStatusRequestHandler.OnServerStatusRequest += RefreshUI;
        RequestServerStatus();
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
    }

    void RequestServerStatus()
    {
        ServerStatusRequest message = new();
        client.SendOnServer(message);
    }
}
