using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Srver
{
    public class ServerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ServerDbContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder options) =>       
            options.UseSqlServer("Data Source=(LocalDB)\\mssqllocaldb; DataBase=Users;Persist Security Info=false; MultipleActiveResultSets=True; Trusted_Connection=True;");
        

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
                         modelBuilder.Entity<User>().ToTable("Users");

        public void SendToDatabase(User user)
        {
            Users.Add(user);
            SaveChanges();
        }
    }
}
