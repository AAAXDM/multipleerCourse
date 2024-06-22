using NetworkShared;
using UnityEngine;
using Zenject;

public class LobbyUI : MonoBehaviour
{
    [Inject] NetworkingClient client;

    void Start()
    {
        RequestServerStatus();
    }

    void RequestServerStatus()
    {
        ServerStatusRequest message = new();
        client.SendOnServer(message);
    }
}
