using Microsoft.Extensions.Logging;
using NetworkShared;

namespace Server.PacketHandlers
{
    [HandlerRegisterAtribute(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
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
            if (sucsess) server.NotiFyAnotherPlayers(connectionId);
            server.SendToClient(connectionId, requestMessage);
        }
    }
}
