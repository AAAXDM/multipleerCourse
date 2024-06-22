using LiteNetLib;

namespace Srver
{
    public class UsersManager
    {
        ServerDbContext db;
        Dictionary<int, ServerConnection> connections;

        public UsersManager(ServerDbContext db)
        {
            connections = new();
            this.db = db;
        }
    
        public void AddConnection(NetPeer peer)
        {
            connections.Add(peer.Id, new ServerConnection
                {
                ConnectionId = peer.Id,
                Peer = peer
                });
        }

        public void Disconnect(int peerId)
        {
            ServerConnection connection = connections[peerId];
            if (connection.User != null)
            {
            }
            connections.Remove(peerId);
        }


        public bool LogIn(int connectionId, string userName, string password)
        {
            User user = db.Users.Where(x => x.UserName == userName).Where(x => x.Password == password).FirstOrDefault();
            if (user != null)
            {
                if (connections.ContainsKey(connectionId))
                {
                    user.IsOnline = true;
                    AddConnection(user, connectionId);
                    db.SaveChanges();
                }
                return true;
            }
            return false;
        }

        public bool Register(int connectionId,string userName, string password)
        {
            User checkUser = db.Users.Where(x => x.UserName == userName).Where(x => x.Password == password).FirstOrDefault();
            if (checkUser == null)
            {
                if (connections.ContainsKey(connectionId))
                {
                    User user = new User();
                    user.UserName = userName;
                    user.Password = password;
                    user.IsOnline = true;
                    AddConnection(user, connectionId);
                    db.SendToDatabase(user);
                }
                return true;
            }

            return false;
        }

        public ServerConnection GetConnection(int connectionId) => connections[connectionId];

        void AddConnection(User user, int connectionId)
        {
            connections[connectionId].User = user;
        }

        public int[] GetOverIds(int excludedId) => connections.Keys.Where(x => x != excludedId).ToArray();
    }
}
