using NetworkShared;
using UnityEngine.SceneManagement;
using Zenject;

[HandlerRegisterAtribute(PacketType.OnStartGame)]
public class OnStartGameHandler : IPacketHandler
{
    [Inject] GameManager gameManager;
    int sceneNumber = 2;

    public void Handle(INetPacket packet, int connectionId)
    {
        gameManager.SetCanPlay(true);
        var msg = (OnStartGame) packet;
        gameManager.RegisterGame(msg.GameId, msg.XUser, msg.OUser);
        SceneManager.LoadScene(sceneNumber);      
    }
}

