using NetworkShared;
using System;
using Zenject;

[HandlerRegisterAtribute(PacketType.OnFinishGame)]
public class FinishGameHandler : IPacketHandler
{
    [Inject] GameManager gameManager;
    public static event Action OnPlayAgain;
    public static event Action OnFinishGame;    

    public void Handle(INetPacket packet, int connectionId)
    {
        gameManager.SetCanPlay(false);
        var msg = (OnFinishGame)packet;
        if (msg.IsFinished)
        {
            OnFinishGame?.Invoke();
        }
        else
        {
            OnPlayAgain?.Invoke();
        }
    }
}

