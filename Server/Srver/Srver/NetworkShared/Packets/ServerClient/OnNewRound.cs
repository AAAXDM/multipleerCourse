using LiteNetLib.Utils;


namespace NetworkShared
{
    public struct OnNewRound : INetPacket
    {
        public PacketType Type => PacketType.OnNewRound;

        public void Deserialize(NetDataReader reader)
        {
        }

        public void Serialize(NetDataWriter writer) => writer.Put((byte)Type);
    }
}
