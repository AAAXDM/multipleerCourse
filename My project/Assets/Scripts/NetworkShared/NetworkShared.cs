using LiteNetLib.Utils;


namespace NetworkShared
{
    public enum PacketType
    {
        #region ClientServer
        Invalid = 0,
        AuthRequest = 1, 
        ServerStatusRequest = 2,
        FindOpponentRequest = 3,
        MarkCellRequest = 4,
        FinishGameRequest = 5,
        #endregion

        #region ServerClient
        OnAuth = 100,
        OnAuthFailed = 101,
        OnServerStatus = 102,
        OnFindOpponent = 103,
        OnFindFaild = 104,
        OnMarkCell = 105,
        OnFinishGame = 106,
        OnNewRound = 107,
        OnStartGame = 108
        #endregion
    }

    public enum AuthRequestType
    {
        Register = 0,
        Auth = 1
    }

    public enum MarkOutcome
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
