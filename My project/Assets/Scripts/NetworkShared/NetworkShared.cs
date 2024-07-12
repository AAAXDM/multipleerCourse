using LiteNetLib.Utils;


namespace NetworkShared
{
    public enum PacketType
    {
        #region ClientServer
        Invalid = 0,
        AuthRequest = 1, 
        ServerStatusRequest = 2,
        FindOpponentrequest = 3,
        #endregion

        #region ServerClient
        OnAuth = 100,
        OnAuthFailed = 101,
        OnServerStatus = 102,
        OnFindOpponent = 103,
        OnStartGame = 104
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
