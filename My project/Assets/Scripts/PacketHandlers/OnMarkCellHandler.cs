using NetworkShared;
using System;
using Zenject;

[HandlerRegisterAtribute(PacketType.OnMarkCell)]
public class OnMarkCellHandler :  IPacketHandler
{
    [Inject] GameManager gameManager;

    public static event Action<OnMarkCell> OnMarkCellEvent;
    public static event Action<OnMarkCell> SurrenderEvent;

    public void Handle(INetPacket packet, int connectionId)
    {
        var msg = (OnMarkCell)packet;
        if (msg.Outcome == MarkOutcome.Win && msg.Result.StartCell.Equals(msg.Result.EndCell))
        {
            SurrenderEvent?.Invoke(msg);
        }
        else
        {
            gameManager.ActiveGame.SwitchActivePlayer();
            OnMarkCellEvent?.Invoke(msg);
        }
    }
}
