using Microsoft.Extensions.Logging;
using NetworkShared;

namespace Srver.PacketHandlers
{
    [HandlerRegisterAtribute(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    { 
        readonly ILogger logger;
        readonly UsersManager manager;

        public AuthRequestHandler(ILogger<AuthRequestHandler> logger, UsersManager usersManager) 
        { 
            this.logger = logger;
            manager = usersManager;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var message = (NetAuthRequest)packet;
            bool isHandeled;
            if (message.RequestType == AuthRequestType.Register)
            {
                isHandeled = manager.Register(connectionId, message.Username, message.Password);
            }
            else
            {
                isHandeled = manager.LogIn(connectionId, message.Username, message.Password);
            }
        }
    }
}
