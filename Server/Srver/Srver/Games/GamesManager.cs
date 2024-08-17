namespace Srver
{
    public class GamesManager
    {
        List<Game> games;

        public GamesManager() 
        {
            games = new();
        }

        public Guid RegisterGame(string xUser, string oUser)
        {
            Game game = new Game(xUser,oUser);
            games.Add(game);
            return game.Id;
        }

        public Game? FindGame(string username) => games.FirstOrDefault(x => x.XUser.UserName == username || x.OUser.UserName == username);

        public void CloseGame(Game game)
        {
            if (game != null) 
            { 
                games.Remove(game);
            }
        }

        public bool GameExists(string username) => games.Any(x => x.XUser.UserName == username || x.OUser.UserName == username);

        public int GamesCount() => games.Count();
        
    }
}
