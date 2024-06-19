using System.Net;
using System.Net.Sockets;
using System.Text;
using LiteNetLib;

namespace Srver
{
    internal class NetworkServer : INetEventListener
    {
        Dictionary<int, NetPeer> connections;
        NetManager netManager;
        int port = 9050;
        int disconnectTimeout = 100000;

        public NetworkServer() 
        {
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
            var data = Encoding.UTF8.GetString(reader.RawData);
            Console.WriteLine(data);
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
    }
}
