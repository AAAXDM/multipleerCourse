using NetworkShared;
using System;


[HandlerRegisterAtribute(PacketType.OnFinishGame)]
public class FinishGameHandler : IPacketHandler
{
    public static event Action OnPlayAgain;
    public static event Action OnFinishGame;    

    public void Handle(INetPacket packet, int connectionId)
    {
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

