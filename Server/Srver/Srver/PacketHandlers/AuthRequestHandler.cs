using Microsoft.Extensions.Logging;
using NetworkShared;

namespace Srver.PacketHandlers
{
    [HandlerRegisterAtribute(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    { 
        readonly ILogger logger;
        readonly UsersManager manager;
        NetworkServer server;

        public AuthRequestHandler(ILogger<AuthRequestHandler> logger, UsersManager usersManager, NetworkServer server) 
        { 
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
            OnServerStatus message = new();

            int[] ids = manager.GetOverIds(excludedPlayerId);

            foreach(var connectionId in ids)
            {
                server.SendToClient(connectionId, message);
            }
        }
    }
}
