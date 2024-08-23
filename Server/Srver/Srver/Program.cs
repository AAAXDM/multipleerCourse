using Microsoft.Extensions.DependencyInjection;
using Server;

class Program
{
    static ServerDbContext db;

    static void Main(string[] args)
    {
        int sleepTime = 15;

        var serviceProvider = Container.Configure();
        var server = serviceProvider.GetRequiredService<NetworkServer>();
        db = new();

        server.Start();

        Thread.GetDomain().UnhandledException += (sender, e) => Exit(null);
        AppDomain.CurrentDomain.ProcessExit += (sender, e) => Exit(null);

        while (true)
        {
            server.PollEvents();
            Thread.Sleep(sleepTime);
        }
    }

    static void Exit(object sender)
    {
        db.SetAllUsersOffline();
        Console.WriteLine($"Server quit");
    }
}

