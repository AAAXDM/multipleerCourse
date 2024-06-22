using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct ServerStatusRequest : INetPacket
    {
        public PacketType Type => PacketType.ServerStatusRequest;

        public void Deserialize(NetDataReader reader)
        {
           
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
