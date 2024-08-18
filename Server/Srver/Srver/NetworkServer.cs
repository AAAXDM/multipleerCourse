using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetworkShared;

namespace Server
{
    public class NetworkServer : INetEventListener
    {
        NetDataWriter netDataWriter = new();
        NetManager netManager;
        UsersManager usersManager;
        ServerDbContext db;
        ILogger<NetworkServer> logger;
        IServiceProvider provider;
        int port = 9050;
        int disconnectTimeout = 100000;
        int topPlayersCount = 8;

        public NetworkServer(ILogger<NetworkServer> logger, IServiceProvider provider, UsersManager usersManager, ServerDbContext db) 
        {
            this.db = db;
            this.usersManager = usersManager;
            this.logger = logger;
            this.provider = provider;

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

        public void PollEvents() => netManager.PollEvents();

        public void OnPeerConnected(NetPeer peer)
        {
            usersManager.AddConnection(peer);
            Console.WriteLine($"Client connected to server {peer.Address}.Id{peer.Id}");
        }

        public void SendToClient(int connectionId, INetPacket message, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
        {
            ServerConnection connection = usersManager.GetConnection(connectionId);
            if (connection != null)
            {
                NetPeer peer = connection.Peer;
                peer.Send(WriteSerializable(message), deliveryMethod);
            }
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            netManager.DisconnectPeer(peer);
            usersManager.Disconnect(peer.Id);
            NotifyAnotherPlayers(peer.Id);
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

        public void NotifyAnotherPlayers(int excludedPlayerId)
        {

            OnServerStatus message = new OnServerStatus
            {
                PlayersCount = (ushort)db.GetOnlinePlayersCount(),
                TopPlayers = db.GetTopUsers(topPlayersCount)
            };

            List<int> ids = usersManager.GetOverIds(excludedPlayerId);

            if (ids != null)
            {
                foreach (var connectionId in ids)
                {
                    SendToClient(connectionId, message);
                }
            }
        }

        INetPacket ResolvePacket(PacketType packetType, NetPacketReader reader)
        {
            PacketRegistry packetRegistry = provider.GetRequiredService<PacketRegistry>();
            var type = packetRegistry.PacketTypes[packetType];
            var packet  = (INetPacket)Activator.CreateInstance(type);
            packet.Deserialize(reader);
            return packet;
        }

        NetDataWriter WriteSerializable(INetPacket packet)
        {
            netDataWriter.Reset();
            packet.Serialize(netDataWriter);
            return netDataWriter;
        }
    }
}
