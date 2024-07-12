using System.Collections.Generic;
using System;
using System.Linq;

namespace NetworkShared
{
    public class PacketRegistry
    {
        Dictionary<PacketType, Type> packetTypes = new();

        public Dictionary<PacketType,Type> PacketTypes 
        { 
            get 
            {
                if(packetTypes.Count == 0)
                {
                    Initialize();
                }                                   
                return packetTypes;             
            } 

        }

        void Initialize()
        {
            var packetType = typeof(INetPacket);
            var packets = AppDomain.CurrentDomain.GetAssemblies()
                .Where(p => !p.FullName.StartsWith("Microsoft"))
                .SelectMany(x => x.GetTypes())
                .Where(p => packetType.IsAssignableFrom(p) && !p.IsInterface);


            bool test = false;

            foreach(var packet in packets)
            {
                INetPacket instance = (INetPacket)Activator.CreateInstance(packet);
                packetTypes.Add(instance.Type, packet);
            }    
        }
    }
}
