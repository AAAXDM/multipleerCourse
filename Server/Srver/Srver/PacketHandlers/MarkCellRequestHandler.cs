using NetworkShared;
using Server;

namespace Srver.PacketHandlers
{
    [HandlerRegisterAtribute(PacketType.MarkCellrequest)]
    public class MarkCellRequestHandler : IPacketHandler
    {
        UsersManager usersManager;
        GamesManager gamesManager;
        NetworkServer server;

        public MarkCellRequestHandler(UsersManager usersManager , GamesManager gamesManager,NetworkServer server )
        {
            this.usersManager = usersManager;
            this.gamesManager = gamesManager;
            this.server = server;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (MarkCellRequest)packet;
            var connection = usersManager.GetConnection(connectionId);
            var userName = connection.User.UserName;
            var game = gamesManager.FindGame(userName);
            string opponent = game.GetOpponent(userName);
            var opConnection = usersManager.GetConnection(opponent);
            Validate(msg.Cell.X,msg.Cell.Y,userName,game);

            var result = game.MarkCell(msg.Cell.X,msg.Cell.Y);

            var rmsg = new OnMarkCell()
            {
                Actor = userName,
                Cell = msg.Cell,
                Outcome = result.MarkOutcome,
                Result = result.WinResult
            };

            server.SendToClient(connectionId,rmsg);
            server.SendToClient(opConnection.ConnectionId,rmsg);
            if(result.MarkOutcome == MarkOutcome.None)
            {
                game.SwitchPlayer();
            }
            if(result.MarkOutcome == MarkOutcome.Win)
            {
                game.AddWin(userName);
                usersManager.IncreaseScore(userName);
            }

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
