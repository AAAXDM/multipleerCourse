namespace Srver
{
    public class UsersManager
    {
        ServerDbContext db;
        Dictionary<int, ServerConnection> connections;

        public UsersManager(ServerDbContext db)
        {
            connections = new();
            this.db = db;
        }
    

        public bool LogIn(int connectionId, string userName, string password)
        {
            User user = db.Users.Where(x => x.UserName == userName).Where(x => x.Password == password).FirstOrDefault();
            if (user != null)
            {
                Console.WriteLine(user.UserName);
                return true;
            }
            return false;
        }

        public bool Register(int connectionId,string userName, string password)
        {
            User checkUser = db.Users.Where(x => x.UserName == userName).Where(x => x.Password == password).FirstOrDefault();
            if (checkUser == null)
            {
                User user = new User();
                user.UserName = userName;
                user.Password = password;
                db.SendToDatabase(user);
                var users = db.Users;
                foreach (var u in users)
                {
                    Console.WriteLine(u.UserName);
                }
                return true;
            }

            return false;
        }
    }
}
