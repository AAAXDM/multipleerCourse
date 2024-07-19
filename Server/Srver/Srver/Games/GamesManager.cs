namespace Srver
{
    public class GamesManager
    {
        List<Game> games;

        public GamesManager() 
        {
            games = new();
        }

        public Guid RegistrGame(string xUser, string oUser)
        {
            Game game = new Game(xUser,oUser);
            games.Add(game);
            return game.Id;
        }

        public Game? FindGame(string username) => games.FirstOrDefault(x => x.XUser == username || x.OUser == username);

        public Game CloseGame(string username)
        {
            Game game = FindGame(username);

            if (game != null) 
            { 
                games.Remove(game);
            }

            return game;
        }

        public bool Gameexists(string username) => games.Any(x => x.XUser == username || x.OUser == username);


        public int GamesCount() => games.Count();
        
    }
}
