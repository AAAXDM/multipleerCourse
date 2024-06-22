using NetworkShared;
using System;


[HandlerRegisterAtribute(PacketType.OnAuthFailed)]
public class OnAuthFailedHandler : IPacketHandler
{
    public static event Action OnAuthFailed;

    public void Handle(INetPacket packet, int connectionId)
    {
        OnAuthFailed?.Invoke();
    }
}
