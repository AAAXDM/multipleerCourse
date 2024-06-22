using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;

namespace NetworkShared
{
    [HandlerRegisterAtribute(PacketType.AuthRequest)]
    public class HandlerRegistry
    {
        Dictionary<PacketType, Type> handlers = new();

        public Dictionary<PacketType, Type> Handlers
        {
            get
            {
                if (handlers.Count == 0)
                {
                    Initialize();
                }
                return handlers;
            }
        }

        void Initialize()
        {
            var handlers = AppDomain.CurrentDomain.GetAssemblies()
                .Where(p => !p.FullName.StartsWith("Microsoft"))
                .SelectMany(x => x.DefinedTypes)
                .Where(x => !x.IsAbstract && !x.IsInterface && !x.IsGenericTypeDefinition)
                .Where(x => typeof(IPacketHandler).IsAssignableFrom(x))
                .Select(t => (type: t, attr: t.GetCustomAttribute<HandlerRegisterAtribute>()))
                .Where(x => x.attr != null);

            foreach (var (type, attr) in handlers)
            { 
                if(!this.handlers.ContainsKey(attr.PacketType))
                {
                    this.handlers[attr.PacketType] = type;
                }
                else
                {
                    throw new Exception($"Multiple handlers for {attr.PacketType} packet type detected!");
                }
            }
        }
    }
}
