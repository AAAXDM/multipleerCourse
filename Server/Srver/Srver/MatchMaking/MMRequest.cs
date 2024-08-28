using Server;


namespace Srver
{
    public class MMRequest
    {
        ServerConnection connection;
        CancellationTokenSource cancellationToken;
        DateTime searchStart;
        bool matchFound;

        public ServerConnection Connection => connection;
        public DateTime SearchStart => searchStart;
        public bool MatchFound => matchFound;

        public MMRequest(ServerConnection connection,DateTime searchStart)
        {
            this.connection = connection;
            this.searchStart = searchStart;        
        }

        public void SetMatchFound(bool matchFound) => this.matchFound = matchFound;

        public void SetCancellationTokenSource(CancellationTokenSource cancellationTokenSource) => cancellationToken = cancellationTokenSource;

        public void Cancel()
        {
            if (cancellationToken != null)
            {
                cancellationToken.Cancel();
                cancellationToken.Dispose();
            }
        }
    }
}
