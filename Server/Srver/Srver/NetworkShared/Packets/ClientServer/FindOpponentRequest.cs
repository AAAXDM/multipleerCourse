using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct FindOpponentRequest : INetPacket
    {
        public PacketType Type => PacketType.FindOpponentrequest;
        public bool NeedToStop { get; set; }

        public void Deserialize(NetDataReader reader) => reader.GetBool();

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(NeedToStop);
        }
    }
}
