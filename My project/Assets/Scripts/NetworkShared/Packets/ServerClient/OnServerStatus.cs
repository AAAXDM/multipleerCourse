﻿using LiteNetLib.Utils;

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
}
