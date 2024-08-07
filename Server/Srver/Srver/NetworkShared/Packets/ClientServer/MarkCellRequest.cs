using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct MarkCellRequest : INetPacket
    {
        public PacketType Type => PacketType.MarkCellrequest;

        public Cell Cell { get; set; }

        public void Deserialize(NetDataReader reader) => Cell = reader.Get<Cell>();

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Cell);
        }
    }
}
