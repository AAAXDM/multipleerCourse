using Microsoft.Extensions.DependencyInjection;
using NetworkShared;
using System.Reflection;


namespace Server.Extentions
{
    public static class ServiceCollectionsExtention
    {
        public static IServiceCollection AddpacketHandlers(this IServiceCollection services)
        {
            var handlers = AppDomain.CurrentDomain.GetAssemblies().
                SelectMany(x => x.DefinedTypes)
                .Where(x => !x.IsAbstract && !x.IsInterface && !x.IsGenericTypeDefinition)
                .Where(x => typeof(IPacketHandler).IsAssignableFrom(x))
                .Select(t => (type: t, attr: t.GetCustomAttribute<HandlerRegisterAtribute>()))
                .Where(x => x.attr != null);

            foreach (var (type, attr) in handlers)
            {
                services.AddScoped(type);
            }

            return services;
        }
    }
}
