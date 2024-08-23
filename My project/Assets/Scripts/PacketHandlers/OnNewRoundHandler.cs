using NetworkShared;
using System;
using Zenject;

[HandlerRegisterAtribute(PacketType.OnNewRound)]
public class OnNewRoundHandler : IPacketHandler
{
    [Inject] GameManager gameManager;

    public static event Action OnNewRound;

    public void Handle(INetPacket packet, int connectionId)
    {
        gameManager.SetCanPlay(true);
        gameManager.ActiveGame.ResetGame();
        OnNewRound?.Invoke();
    }
}

