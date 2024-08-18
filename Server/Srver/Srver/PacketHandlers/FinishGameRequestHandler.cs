using NetworkShared;
using Server;

namespace Srver.PacketHandlers
{
    [HandlerRegisterAtribute(PacketType.FinishGameRequest)]
    public class FinishGameRequestHandler : IPacketHandler
    {
        UsersManager usersManager;
        GamesManager gamesManager;
        NetworkServer networkServer;

        public FinishGameRequestHandler
            (UsersManager usersManager, GamesManager gamesManager, 
                                       NetworkServer networkServer)
        {
            this.usersManager = usersManager;
            this.gamesManager = gamesManager;
            this.networkServer = networkServer;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            FinishGameRequest req = (FinishGameRequest)packet;
            var connection = usersManager.GetConnection(connectionId);
            var userName = connection.User.UserName;
            if (!gamesManager.GameExists(userName)) return;
            INetPacket rmsg;
            var game = gamesManager.FindGame(userName);
            var opConnection = GetOpponentConnection(userName, game);
            if (req.IsFinished)
            {
                gamesManager.CloseGame(game);
                rmsg = new OnFinishGame()
                {
                    IsFinished = true
                };
            }
            else
            {
                bool playAgain = game.CanPlayAgain(userName);
                if (playAgain)
                {
                    rmsg = new OnNewRound();
                    networkServer.SendToClient(connectionId, rmsg);
                    game.CreateNewRound();
                }
                else
                {
                    rmsg = new OnFinishGame()
                    {
                        IsFinished = false
                    };
                }
            }
            if (opConnection != null)
            {
                networkServer.SendToClient(opConnection.ConnectionId, rmsg);
            }
        }

        ServerConnection GetOpponentConnection(string userName, Game game)
        {
            string opponent = game.GetOpponent(userName);
            return usersManager.GetConnection(opponent);
        }
    }
}
