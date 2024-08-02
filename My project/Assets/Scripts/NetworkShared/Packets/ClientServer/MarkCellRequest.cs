using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct MarkCellRequest : INetPacket
    {
        public PacketType Type => PacketType.MarkCellrequest;

        public byte Row { get; set; }
        public byte Col { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Row = reader.GetByte();
            Col = reader.GetByte();
        }


        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Row);
            writer.Put(Col);
        }
    }
}
