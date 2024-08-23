using Microsoft.Identity.Client;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Srver
{
    public class MMRequest
    {
        ServerConnection connection;
        DateTime searchStart;
        bool matchFound;

        public ServerConnection Connection => connection;
        public DateTime SearchStart => searchStart;
        public bool MatchFound => matchFound;

        public MMRequest(ServerConnection connection,DateTime searchStart)
        {
            this.connection = connection;
            this.searchStart = searchStart;        
        }

        public void SetMatchFound(bool matchFound) => this.matchFound = matchFound;
    }
}
