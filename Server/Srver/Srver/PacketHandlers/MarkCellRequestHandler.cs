using NetworkShared;
using Server;

namespace Srver.PacketHandlers
{
    [HandlerRegisterAtribute(PacketType.MarkCellRequest)]
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
            string opponent = game?.GetOpponent(userName);
            var opConnection = usersManager.GetConnection(opponent);
            OnMarkCell rmsg;

            if (msg.IsSurrendering)
            {
                rmsg = new OnMarkCell()
                {
                    Actor = opponent,
                    Outcome = MarkOutcome.Win
                };
                SendRequest(connectionId, rmsg, opConnection, game, opponent);
            }
            else
            {
                if (Validate(msg.Cell.X, msg.Cell.Y, userName, game))
                {
                    var result = game.MarkCell(msg.Cell.X, msg.Cell.Y);
                    rmsg = new OnMarkCell()
                    {
                        Actor = userName,
                        Cell = msg.Cell,
                        Outcome = result.MarkOutcome,
                        Result = result.WinResult
                    };

                    SendRequest(connectionId,rmsg,opConnection,game, userName);
                }
            }
        }

        bool Validate(byte row,byte col, string userName,Game game)
        {
            if(game.CurrentUser != userName || game.Grid[row, col] != 0)
            {
                return false;
            }

            return true;
        }

        void SendRequest(int connectionId,OnMarkCell rmsg, ServerConnection opConnection,Game game ,string winner)
        {
            server.SendToClient(connectionId, rmsg);
            if (opConnection != null)
            {
                server.SendToClient(opConnection.ConnectionId, rmsg);
            }

            if (rmsg.Outcome == MarkOutcome.None)
            {
                game?.SwitchPlayer();
            }
            if (rmsg.Outcome == MarkOutcome.Win)
            {
                game.AddWin(winner);
                usersManager.IncreaseScore(winner);
            }
        }
    }
}
