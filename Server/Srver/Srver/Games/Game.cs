using NetworkShared;

namespace Srver
{
    public class Game
    {
        int maxSize = 3;
        int center;
        byte halfcountToWin = 1;

        public Guid Id { get; set; }
        public ushort Round { get; set; }
        public string OUser { get; set; }
        public ushort OWins { get; set; }
        public bool OWantsRemach { get; set; }
        public string XUser { get; set; }
        public bool XWanteRematch { get; set; }
        public string CurrentUser { get; set; }
        public MarkType[,] Grid { get; }

        public Game(string xUser, string oUser)
        {
            Id = new Guid();
            OUser = oUser;
            XUser = xUser;
            Round = 1;
            CurrentUser = XUser;
            Grid = new MarkType[maxSize, maxSize];
            center = maxSize / 2;
        }

       /* public MarkResult MarkCell(byte row, byte column)
        {
            Grid[row, column] = GetPlayerType(CurrentUser);
        }*/

        MarkType GetPlayerType(string userName)
        {
            if(userName == XUser)
            {
                return MarkType.X;
            }
            if(userName == OUser) 
            {
                return MarkType.O;
            }
            return MarkType.None;
        }

        void CheckWin(byte row, byte col) 
        {
            byte center = (byte)this.center;
            byte minRow = row <= center ? (byte)(center - halfcountToWin) : (byte)(row - center);
            byte minCol;
            byte maxRow;
            byte maxCol;
        }
    }

    public struct MarkResult
    {
        public MarckOutcome MarckOutcome { get; set; }
        public WinLineType? WinLine { get; set; }

        public (byte,byte) StartCell { get; set; }
        public (byte, byte) EndCell { get; set; }
    }
}
