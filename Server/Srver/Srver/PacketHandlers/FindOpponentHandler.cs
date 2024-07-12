using NetworkShared;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Srver.PacketHandlers
{
    [HandlerRegisterAtribute(PacketType.FindOpponentrequest)]
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
           var connection = usersManager.GetConnection(connectionId);
            matchMaker.RegisterPlayer(connection);
        }
    }
}
