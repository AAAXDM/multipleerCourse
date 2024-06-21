using LiteNetLib.Utils;

namespace NetworkShared
{
    public class NetAuthRequest : INetPacket
    {
        public PacketType Type => PacketType.AuthRequest;

        public AuthRequestType RequestType { get; private set; }

        public string Username { get; private set; }
        public string Password { get; private set; }


        public NetAuthRequest(string username, string password, AuthRequestType requestType)
        {
            RequestType = requestType;
            Username = username;
            Password = password;
            RequestType = requestType;
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
