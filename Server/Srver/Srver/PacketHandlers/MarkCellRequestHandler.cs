using NetworkShared;
using Server;
using System.Data;

namespace Srver.PacketHandlers
{
    [HandlerRegisterAtribute(PacketType.MarkCellrequest)]
    public class MarkCellRequestHandler : IPacketHandler
    {
        UsersManager usersManager;
        GamesManager gamesManager;

        public MarkCellRequestHandler(UsersManager usersManager , GamesManager gamesManager )
        {
            this.usersManager = usersManager;
            this.gamesManager = gamesManager;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (MarkCellRequest)packet;
            var connection = usersManager.GetConnection(connectionId);
            var userName = connection.User.UserName;
            var game = gamesManager.FindGame(userName);
            Validate(msg.Row,msg.Col,userName,game);
        }

        void Validate(byte row,byte col, string userName,Game game)
        {
            if(game.CurrentUser != userName)
            {
                throw new ArgumentException($"bad request actor {userName} is not the current user!");
            }


            if (game.Grid[row,col] != 0)
            {
                throw new ArgumentException($"bad request cell at row {row} and column {col} is alredy marked!");
            }
        }
    }
}
