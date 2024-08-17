using LiteNetLib.Utils;


namespace NetworkShared
{
    public struct OnFinishGame : INetPacket
    {
        public PacketType Type => PacketType.OnFinishGame;

        public bool IsFinished { get; set; }

        public void Deserialize(NetDataReader reader) => IsFinished = reader.GetBool();

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(IsFinished);
        }
    }
}
