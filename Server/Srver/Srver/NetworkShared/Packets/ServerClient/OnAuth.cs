using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct OnAuth : INetPacket
    {
        public PacketType Type => PacketType.OnAuth;

        public void Deserialize(NetDataReader reader)
        {
        }

        public void Serialize(NetDataWriter writer) => writer.Put((byte)Type);
    }
}
