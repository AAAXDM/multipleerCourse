using LiteNetLib.Utils;


namespace NetworkShared
{
    public struct OnFindFailed : INetPacket
    {
        public PacketType Type => PacketType.OnFindFaild;

        public void Deserialize(NetDataReader reader)
        {
        }

        public void Serialize(NetDataWriter writer) => writer.Put((byte)Type);
    }
}
