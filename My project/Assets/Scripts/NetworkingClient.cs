using LiteNetLib;
using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Net;
using System.Net.Sockets;
using Zenject;

public class NetworkingClient :  IInitializable, ITickable, IDisposable, INetEventListener
{
    readonly NetworkingClientSettings settings;
    NetManager netManager;
    NetPeer peer;
    NetDataWriter dataWriter;
    PacketRegistry packetRegistry;
    HandlerRegistry handlerRegistry;

    public event Action OnServerConnected;

    public NetworkingClient(NetworkingClientSettings settings)
    {
        this.settings = settings;
    }

    public void Initialize() => Init();

    public void Tick() => netManager.PollEvents();

    public void Dispose()
    {
        if (peer != null)
        {
            netManager.Stop();
        }
        Disconnect();
    }

    void Init()
    {
        packetRegistry = new PacketRegistry();
        handlerRegistry = new HandlerRegistry();
        dataWriter = new ();
        netManager = new(this)
        {
            DisconnectTimeout = settings.disconnectTimeout
        };
        netManager.Start();
    }

    public void Connect()
    {
        netManager.Connect("localhost", settings.port, "");
    }

    public void SendOnServer<T>(T packet,DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : INetSerializable
    {
        if (peer != null)
        {
            dataWriter.Reset();
            packet.Serialize(dataWriter);
            peer.Send(dataWriter, deliveryMethod);
        }
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        PacketType packetType = (PacketType)reader.GetByte();
        INetPacket packet = ResolvePacket(packetType, reader);
        IPacketHandler handler = ResolveHandler(packetType);
        handler.Handle(packet,peer.Id);
        reader.Recycle();
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
    }

    public void OnPeerConnected(NetPeer peer)
    {
       this.peer = peer;
       OnServerConnected?.Invoke();
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
    }

    public void Disconnect() => netManager.DisconnectAll();

    INetPacket ResolvePacket(PacketType packetType,NetPacketReader reader)
    {
        var type = packetRegistry.PacketTypes[packetType];
        var packet = (INetPacket)Activator.CreateInstance(type);
        packet.Deserialize(reader);
        return packet;
    }

    IPacketHandler ResolveHandler(PacketType packetType)
    {
        var handlerType = handlerRegistry.Handlers[packetType];
        return (IPacketHandler)Activator.CreateInstance(handlerType);
    }
}

[Serializable]
public class NetworkingClientSettings
{
    public int disconnectTimeout;
    public int port;
}