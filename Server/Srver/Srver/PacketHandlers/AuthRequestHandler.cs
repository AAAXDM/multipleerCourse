using Microsoft.Extensions.Logging;
using NetworkShared;

namespace Srver.PacketHandlers
{
    [HandlerRegisterAtribute(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        int topPlayersCount = 8;
        readonly ILogger logger;
        readonly UsersManager manager;
        NetworkServer server;
        ServerDbContext db;

        public AuthRequestHandler(ILogger<AuthRequestHandler> logger, UsersManager usersManager, NetworkServer server, ServerDbContext  db) 
        { 
            this.db = db;
            this.server = server;
            this.logger = logger;
            manager = usersManager;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var message = (NetAuthRequest)packet;
            INetPacket requestMessage;
            bool sucsess;


            if (message.RequestType == AuthRequestType.Register)
            {
                sucsess = manager.Register(connectionId, message.Username, message.Password);
            }
            else
            {
                sucsess = manager.LogIn(connectionId, message.Username, message.Password);
            }

            requestMessage = sucsess ? new OnAuth() : new OnAuthFailed();
            if (sucsess) NotiFyAnotherPlayers(connectionId);
            server.SendToClient(connectionId, requestMessage);
        }

        void NotiFyAnotherPlayers(int excludedPlayerId)
        {
            OnServerStatus message = new OnServerStatus
            {
                PlayersCount = (ushort)db.Users.Count(),
                TopPlayers = db.GetTopUsers(topPlayersCount)
            };

            int[] ids = manager.GetOverIds(excludedPlayerId);

            foreach(var connectionId in ids)
            {
                server.SendToClient(connectionId, message);
            }
        }
    }
}
