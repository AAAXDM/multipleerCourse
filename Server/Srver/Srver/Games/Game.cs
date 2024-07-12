using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Srver
{
    public class Game
    {
        public Guid Id { get; set; }
        public ushort Round { get; set; }

        public string OUser { get; set; }

        public ushort OWins { get; set; }

        public bool OWantsRemach { get; set; }

        public string XUser { get; set; }

        public bool XWanteRematch { get; set; }

        public string CurrentUser { get; set; }

        public Game(string xUser, string oUser)
        {
            Id = new Guid();
            OUser = oUser;
            XUser = xUser;
            Round = 1;
            CurrentUser = XUser;
        }
    }
}
