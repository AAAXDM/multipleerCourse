using NetworkShared;

namespace Server.PacketHandlers
{
    [HandlerRegisterAtribute(PacketType.ServerStatusRequest)]
    public class ServerStatusRequestHandler : IPacketHandler
    {
        ServerDbContext db;
        NetworkServer server;
        int usersCount = 8;

        public ServerStatusRequestHandler(NetworkServer server, ServerDbContext db)
        {
            this.db = db;
            this.server = server;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            OnServerStatus message = new OnServerStatus
            {
                PlayersCount = (ushort)db.Users.Count(),
                TopPlayers = db.GetTopUsers(usersCount)
            };

            server.SendToClient(connectionId, message);
        }
    }
}
