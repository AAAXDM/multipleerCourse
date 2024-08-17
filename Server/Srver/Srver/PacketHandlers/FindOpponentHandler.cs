using NetworkShared;
using Server;


namespace Srver.PacketHandlers
{
    [HandlerRegisterAtribute(PacketType.FindOpponentRequest)]
    public class FindOpponentHandler : IPacketHandler
    {
        UsersManager usersManager;
        MatchMaker matchMaker;

        public FindOpponentHandler(UsersManager usersManager, MatchMaker matchMaker) 
        { 
            this.usersManager = usersManager;
            this.matchMaker = matchMaker;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var message = (FindOpponentRequest)packet;
            var connection = usersManager.GetConnection(connectionId);
            if (!message.NeedToStop)
            {
                if (connection != null)
                {
                    matchMaker.RegisterPlayer(connection);
                }
            }
            else
            {
                if (connection != null)
                {
                    matchMaker.TryUnregisterPlayer(connection.User.UserName);
                }
            }
        }
    }
}
