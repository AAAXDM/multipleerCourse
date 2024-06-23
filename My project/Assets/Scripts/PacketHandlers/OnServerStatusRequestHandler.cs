using NetworkShared;
using System;
using UnityEngine.SceneManagement;

[HandlerRegisterAtribute(PacketType.OnServerStatus)]
public class OnServerStatusRequestHandler : IPacketHandler
{
    int lobbySceneIndex = 1;

    public static event Action<OnServerStatus> OnServerStatusRequest;

    public void Handle(INetPacket packet, int connectionId)
    {
        if (SceneManager.GetActiveScene().buildIndex == lobbySceneIndex)
        {
            var message = (OnServerStatus)packet;
            OnServerStatusRequest?.Invoke(message);
        }
    }
}
