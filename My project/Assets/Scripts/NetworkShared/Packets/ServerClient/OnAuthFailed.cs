using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct OnAuthFailed : INetPacket
    {
        public PacketType Type => PacketType.OnAuthFailed;

        public void Deserialize(NetDataReader reader)
        {
        }

        public void Serialize(NetDataWriter writer) => writer.Put((byte)Type);
    }
}
