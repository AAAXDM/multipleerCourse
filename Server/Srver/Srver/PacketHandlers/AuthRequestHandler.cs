using NetworkShared;

namespace Server.PacketHandlers
{
    [HandlerRegisterAtribute(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        readonly UsersManager manager;
        NetworkServer server;

        public AuthRequestHandler(UsersManager usersManager, NetworkServer server) 
        { 
            this.server = server;
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
