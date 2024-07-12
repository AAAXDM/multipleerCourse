using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetworkShared;
using Server.Extentions;
using Srver;

namespace Server
{
    internal static class Container
    {
        public static IServiceProvider Configure()
        {
            ServiceCollection services = new ();
            ConfigureServices(services);
            return services.BuildServiceProvider();
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(x => x.AddSimpleConsole());
            services.AddSingleton<NetworkServer>();
            services.AddSingleton<PacketRegistry>();
            services.AddSingleton<HandlerRegistry>();
            services.AddSingleton<UsersManager>();
            services.AddSingleton<MatchMaker>();
            services.AddSingleton<GamesManager>();
            services.AddDbContext<ServerDbContext>();
            services.AddpacketHandlers();
        }
    }
}
