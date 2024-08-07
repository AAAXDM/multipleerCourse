using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct Cell : INetSerializable
    {
        public byte X;
        public byte Y;

        public void Deserialize(NetDataReader reader)
        {
            X = reader.GetByte();
            Y = reader.GetByte();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(X);
            writer.Put(Y);
        }
    }
    public struct WinResult : INetSerializable
    {
        public Cell StartCell { get; set; }
        public Cell EndCell { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            StartCell = reader.Get<Cell>();
            EndCell = reader.Get<Cell>();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(StartCell);
            writer.Put(EndCell);
        }
    }

    public struct NetPlayerDto : INetSerializable
    {
        public string Username { get; set; }
        public ushort Score { get; set; }
        public bool IsOnline { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Username = reader.GetString();
            Score = reader.GetUShort();
            IsOnline = reader.GetBool();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Username);
            writer.Put(Score);
            writer.Put(IsOnline);
        }
    }
}
