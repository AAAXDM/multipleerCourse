using NetworkShared;
using UnityEngine.SceneManagement;

[HandlerRegisterAtribute(PacketType.OnAuth)]
public class OnAuthHandler : IPacketHandler
{
    int sceneNumber = 1;

    public void Handle(INetPacket packet, int connectionId)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
