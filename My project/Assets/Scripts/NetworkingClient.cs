using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkingClient : MonoBehaviour,INetEventListener
{
    [SerializeField] int disconnectTimeout;
    [SerializeField] int port;
    NetManager netManager;
    NetPeer peer;
    NetDataWriter dataWriter;

    public event Action OnServerConnected;

    void Start() => Init();

    void Update() => netManager.PollEvents();

    void Init()
    {
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
        string data = Encoding.UTF8.GetString(reader.RawData);

        string reply = "reply from client";
        var bytes = Encoding.UTF8.GetBytes(reply);
        peer.Send(bytes,DeliveryMethod.ReliableOrdered);
        Debug.Log(data);
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
}
