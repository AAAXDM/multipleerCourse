using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct MarkCellRequest : INetPacket
    {
        public PacketType Type => PacketType.MarkCellRequest;
        public Cell Cell { get; set; }
        public bool IsSurrendering { get; set;}

        public void Deserialize(NetDataReader reader)
        {
            Cell = reader.Get<Cell>();
            IsSurrendering = reader.GetBool();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Cell);
            writer.Put(IsSurrendering);
        }
    }
}
