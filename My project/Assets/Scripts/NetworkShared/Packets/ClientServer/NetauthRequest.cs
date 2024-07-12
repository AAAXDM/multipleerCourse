using LiteNetLib.Utils;

namespace NetworkShared
{
    public struct NetAuthRequest : INetPacket
    {
        public PacketType Type => PacketType.AuthRequest;
        public AuthRequestType RequestType { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }


        public NetAuthRequest(string username, string password, AuthRequestType type)
        {
            RequestType = type; 
            Username = username;
            Password = password;
        }

        public void Deserialize(NetDataReader reader)
        {
            RequestType = (AuthRequestType)reader.GetByte();
            Username = reader.GetString();
            Password = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put((byte)RequestType);
            writer.Put(Username);
            writer.Put(Password);
        }
    }
}
