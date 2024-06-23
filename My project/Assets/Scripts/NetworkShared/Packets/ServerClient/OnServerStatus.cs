using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct OnServerStatus : INetPacket
    {
        public PacketType Type => PacketType.OnServerStatus;
        public ushort PlayersCount {  get; set; }
        public NetPlayerDto[] TopPlayers { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            PlayersCount = reader.GetUShort();

            ushort topPlayerLength = reader.GetUShort();
            TopPlayers = new NetPlayerDto[topPlayerLength];

            for (int i = 0; i < TopPlayers.Length; i++)
            {
                TopPlayers[i] = reader.Get<NetPlayerDto>();
            }
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(PlayersCount);

            writer.Put((ushort)TopPlayers.Length);
            for (int i = 0; i < TopPlayers.Length; i++)
            {
                writer.Put(TopPlayers[i]);
            }
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
