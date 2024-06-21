using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetworkShared;

namespace Srver
{
    internal class NetworkServer : INetEventListener
    {
        Dictionary<int, NetPeer> connections;
        NetManager netManager;
        ILogger<NetworkServer> logger;
        IServiceProvider provider;
        int port = 9050;
        int disconnectTimeout = 100000;

        public NetworkServer(ILogger<NetworkServer> logger, IServiceProvider provider) 
        {
            this.logger = logger;
            this.provider = provider;
            connections = new ();

            netManager = new(this)
            {
                DisconnectTimeout = disconnectTimeout
            };
        }  

        public void Start()
        {
            netManager.Start(port);
            Console.WriteLine($"Server listening on port {port}");
        }

        public void PollEvents()
        {
            netManager.PollEvents();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            connections.Add(peer.Id, peer);
            Console.WriteLine($"Client connected to server {peer.Address}.Id{peer.Id}");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            connections.Remove(peer.Id);
            Console.WriteLine($"Client disconnected to server {peer.Address}.Id{peer.Id}");
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {

        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            using(var scope = provider.CreateScope())
            {
                try
                {
                    PacketType packetType = (PacketType)reader.GetByte();
                    INetPacket packet = ResolvePacket(packetType, reader);
                    IPacketHandler handler = ResolveHandler(packetType);
                    handler.Handle(packet, peer.Id);
                    reader.Recycle();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex,"Error");
                }
            }
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {

        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            request.Accept();
            Console.WriteLine($"Incoming connection from {request.RemoteEndPoint}");
        }

        public IPacketHandler ResolveHandler(PacketType packetType)
        {
            var registry = provider.GetRequiredService<HandlerRegistry>();
            Type type = registry.Handlers[packetType];
            return (IPacketHandler) provider.GetRequiredService(type);
        }

        INetPacket ResolvePacket(PacketType packetType, NetPacketReader reader)
        {
            PacketRegistry packetRegistry = provider.GetRequiredService<PacketRegistry>();
            var type = packetRegistry.PacketTypes[packetType];
            var packet  = (INetPacket)Activator.CreateInstance(type);
            packet.Deserialize(reader);
            return packet;
        }
    }
}
