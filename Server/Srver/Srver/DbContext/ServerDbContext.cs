using Microsoft.EntityFrameworkCore;
using NetworkShared;

namespace Server
{
    public class ServerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ServerDbContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder options) =>       
            options.UseSqlServer("Data Source=(LocalDB)\\mssqllocaldb; DataBase=Users2;Persist Security Info=false; MultipleActiveResultSets=True; Trusted_Connection=True;");
        

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
                         modelBuilder.Entity<User>().ToTable("Users");

        public void SendToDatabase(User user)
        {
            Users.Add(user);
            SaveChanges();
        }

        public int GetOnlinePlayersCount() => Users.Where(x => x.IsOnline).Count();

        public NetPlayerDto[] GetTopUsers(int count)
        {
            return Users.OrderByDescending(x => x.Score)
                        .Take(count)
                        .Select(x =>
                                new NetPlayerDto
                                {
                                    Username = x.UserName, 
                                    Score = x.Score,
                                    IsOnline = x.IsOnline
                                }

                        ).ToArray();
        }

        public void SetUserOffline(User user)
        {
            User userToOffline = Users.Where(x => x == user).FirstOrDefault();
            if(userToOffline != null)
            {
                user.IsOnline = false;
                SaveChanges();
            }
        }
    }
}
