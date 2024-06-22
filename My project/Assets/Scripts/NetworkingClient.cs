using LiteNetLib;
using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetworkingClient : MonoBehaviour,INetEventListener
{
    [SerializeField] int disconnectTimeout;
    [SerializeField] int port;
    NetManager netManager;
    NetPeer peer;
    NetDataWriter dataWriter;
    PacketRegistry packetRegistry;
    HandlerRegistry handlerRegistry;

    public event Action OnServerConnected;

    void Start() => Init();

    void Update() => netManager.PollEvents();

    void Init()
    {
        packetRegistry = new PacketRegistry();
        handlerRegistry = new HandlerRegistry();
        dataWriter = new ();
        netManager = new(this)
        {
            DisconnectTimeout = disconnectTimeout
        };
        netManager.Start();
    }

    public void Connect()
    {
        netManager.Connect("localhost", port, "");
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
