using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct OnMarkCell : INetPacket
    {
        public PacketType Type => PacketType.OnMarkCell;

        public string Actor {get;set;}
        public Cell Cell { get;set;}
        public MarkOutcome Outcome { get;set;}
        public WinResult Result { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Actor = reader.GetString();
            Cell = reader.Get<Cell>();
            Outcome = (MarkOutcome)reader.GetByte();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Actor);
            writer.Put(Cell); 
            writer.Put((byte)Outcome);
        }
    }
}
