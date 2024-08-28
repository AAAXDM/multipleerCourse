using NetworkShared;
using Server;

namespace Srver
{
    public class MatchMaker
    {
        List<MMRequest> pool;
        GamesManager gamesManager;
        NetworkServer server;

        public MatchMaker(GamesManager gamesManager, NetworkServer server)
        {
            pool = new();
            this.gamesManager = gamesManager;
            this.server = server;
        }

        public void RegisterPlayer(ServerConnection connection)
        {
            if(!pool.Any(x => x.Connection.User.Id == connection.User.Id))
            {
                MMRequest request = new MMRequest(connection, DateTime.UtcNow);
                pool.Add(request);
                DoMatchMaking(request);
            }
        }

        public void TryUnregisterPlayer(string username)
        {
            MMRequest? request = pool.FirstOrDefault(x => x.Connection.User.UserName == username);
            if (request != null) 
            {
                request.Cancel();
                pool.Remove(request);
            }
        }

        async Task DoMatchMaking(MMRequest request)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            request.SetCancellationTokenSource(source);
            await Task.Delay(4000,cancellationToken:source.Token);
            MMRequest? match = pool.FirstOrDefault(x => !x.MatchFound && x.Connection.ConnectionId != request.Connection.ConnectionId);
            if (match == null) 
            {
                OnFindFailed onFindFailed = new();
                pool.Remove(request);
                server.SendToClient(request.Connection.ConnectionId, onFindFailed);
                return;
            }

            request.SetMatchFound(true);
            match.SetMatchFound(true);
            Guid gameId = gamesManager.RegisterGame(request.Connection.User.UserName, match.Connection.User.UserName);
            request.Connection.GameId  = gameId;
            match.Connection.GameId = gameId;
            OnStartGame onStartGame = new OnStartGame
            {
                XUser = request.Connection.User.UserName,
                OUser = match.Connection.User.UserName,
                GameId = gameId,
            };
            pool.Remove(request);
            pool.Remove(match);
            server.SendToClient(request.Connection.ConnectionId, onStartGame);
            server.SendToClient(match.Connection.ConnectionId, onStartGame);           
        }
    }
}
