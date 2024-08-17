using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct FinishGameRequest : INetPacket
    {
        public PacketType Type => PacketType.FinishGameRequest;
        public bool IsFinished { get; set;}

        public void Deserialize(NetDataReader reader) => IsFinished = reader.GetBool();

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(IsFinished);
        }
    }
}
