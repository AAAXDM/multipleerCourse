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
        MarkCellrequest = 4,
        #endregion

        #region ServerClient
        OnAuth = 100,
        OnAuthFailed = 101,
        OnServerStatus = 102,
        OnFindOpponent = 103,
        OnStartGame = 104,
        OnMarkCell = 105
        #endregion
    }

    public enum AuthRequestType
    {
        Register = 0,
        Auth = 1
    }

    public enum MarckOutcome
    {
        None, 
        Win,
        Draw
    }

    public enum MarkType
    {
        None,
        X = 1,
        O = 2
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
