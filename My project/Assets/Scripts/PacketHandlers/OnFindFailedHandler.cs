using NetworkShared;
using System;

[HandlerRegisterAtribute(PacketType.OnFindFaild)]
public class OnFindFailedHandler : IPacketHandler
{
    public static event Action FindFaildEvent;

    public void Handle(INetPacket packet, int connectionId) => FindFaildEvent?.Invoke();
}

