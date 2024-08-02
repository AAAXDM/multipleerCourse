using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct OnMarkCell : INetPacket
    {
        public PacketType Type => PacketType.OnMarkCell;

        public string Actor {get;set;}
        public byte Index { get;set;}

        public MarckOutcome Outcome { get;set;}

        public void Deserialize(NetDataReader reader)
        {
            Actor = reader.GetString();
            Index = reader.GetByte();
            Outcome = (MarckOutcome)reader.GetByte();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Actor);
            writer.Put(Index);
            writer.Put((byte)Outcome);
        }
    }
}
