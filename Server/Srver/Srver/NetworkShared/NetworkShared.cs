using LiteNetLib.Utils;


namespace NetworkShared
{
    public enum PacketType
    {
        #region ClientServer
        Invalid = 0,
        AuthRequest = 1, 
        #endregion

        #region ServerClient
        Onauth = 100
        #endregion
    }

    public enum AuthRequestType
    {
        Register = 0,
        Auth = 1
    }

    public interface INetPacket : INetSerializable
    {
        PacketType Type { get; }
    }

    public interface IPacketHandler
    {
        void Handle(INetPacket packet, int connectionId);
    }
}
