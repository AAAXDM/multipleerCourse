using NetworkShared;
using System;
using Zenject;

[HandlerRegisterAtribute(PacketType.OnMarkCell)]
public class OnMarkCellHandler :  IPacketHandler
{
    [Inject] GameManager gameManager;
    public static event Action<OnMarkCell> OnMarkCellEvent;

    public void Handle(INetPacket packet, int connectionId)
    {
        var msg = (OnMarkCell)packet;
        gameManager.ActiveGame.SwitchActivePlayer();
        OnMarkCellEvent?.Invoke(msg);
    }
}
