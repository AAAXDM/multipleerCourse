using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct OnServerStatus : INetPacket
    {
        public PacketType Type => PacketType.OnServerStatus;

        public void Deserialize(NetDataReader reader)
        {
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
