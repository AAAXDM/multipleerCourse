using NetworkShared;

namespace Srver
{
    public class Game
    {
        int maxSize = 3;
        int countToWin = 3;
        byte halfСountToWin = 1;

        public Guid Id { get; set; }
        public ushort Round { get; set; }
        public string XUser { get; set; }
        public string OUser { get; set; }
        public ushort XWins { get; set; }
        public ushort OWins { get; set; }
        public bool OWantsRematch { get; set; }
        public bool XWantsRematch { get; set; }
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
        }

        public MarkResult MarkCell(byte row, byte column)
        {
            MarkType type = GetPlayerType(CurrentUser);
            MarkResult result = new();
            Grid[row, column] = type;
            bool win = CheckWin(row, column, type, out WinResult winResult);
            if(win)
            {
                result.MarkOutcome = MarkOutcome.Win;
                result.WinResult = winResult;
            }
            else
            {
                bool draw = CheckDraw();
                if(draw)
                {
                    result.MarkOutcome = MarkOutcome.Draw;
                }
            }

            return result;
        }
        
        public void SwitchPlayer() => CurrentUser = GetOpponent(CurrentUser);

        public void AddWin(string userName)
        {
            MarkType winnerType = GetPlayerType(userName);
            if (winnerType == MarkType.X) XWins++;
            else OWins++;
        }

        public string GetOpponent(string userName)
        {
            if (userName == XUser) return OUser;

            return XUser;
        }

        MarkType GetPlayerType(string userName)
        {
            if (userName == XUser)
            {
                return MarkType.X;
            }
            if (userName == OUser)
            {
                return MarkType.O;
            }
            return MarkType.None;
        }

        bool CheckWin(byte row, byte col, MarkType type, out WinResult result)
        {
            result = new();
            byte minRow = row <= halfСountToWin ? (byte)(0) : (byte)(2);
            byte minCol = col <= halfСountToWin ? (byte)(0) : (byte)(2);
            byte maxRow = row + halfСountToWin >= maxSize ? (byte)(maxSize - 1) : (byte)(row + halfСountToWin);
            byte maxCol = col + halfСountToWin >= maxSize ? (byte)(maxSize - 1) : (byte)(col + halfСountToWin);
            int j = 1;

            if(CheckLine(minRow, maxRow, col,type, true))
            {
                result.StartCell = new Cell() { X = minRow, Y = col };
                result.EndCell = new Cell() { X = maxRow, Y = col };
                return true;
            }

            if(CheckLine(minCol,maxCol,row,type,false))
            {
                result.StartCell = new Cell() { X = row, Y = minCol };
                result.EndCell =new Cell() { X = row, Y = maxCol };
                return true;
            }

            for (int i = 0; i <= 2; i++)
            {
                if (Grid[minRow + j, minCol + j] != type) break;
                if (j == countToWin)
                {
                    result.StartCell = new Cell() { X = minRow, Y = minCol };
                    result.EndCell = new Cell() { X = (byte)(minRow + j), Y = (byte)(minCol + j) };
                    return true;
                }
            }

            for (int i = 0; i <= 2; i++)
            {
                if (Grid[minRow + j, maxCol - j] != type) break;
                if (j == countToWin)
                {
                    result.StartCell = new Cell() { X = minRow, Y = maxCol };
                    result.EndCell = new Cell() { X = (byte)(minRow + j), Y = (byte)(maxCol - j) };
                    return true;
                }
            }

            return false;
        }

        bool CheckLine(int min,int max,int staticNumber, MarkType type, bool horizontal)
        {
            int j = 1;
            int x = horizontal ? 0 : staticNumber;
            int y = horizontal ? staticNumber : 0;

            for (int i = 0; i <= 2; i++)
            {
                if(horizontal)
                {
                    x = i;
                }
                else
                {
                    y = i;
                }
                if (Grid[x, y] != type) break;

                if (j == countToWin)return true;

                j++;
            }

            return false;
        }

        bool CheckDraw()
        {
            for(int i = 0; i < maxSize; i++)
            {
                for(int j = 0; j < maxSize; j++)
                {
                    if (Grid[i, j] == 0) return false;
                }
            }

            return true;
        }
    }

    public struct MarkResult
    {
        public MarkOutcome MarkOutcome { get; set; }
        public WinResult WinResult { get; set; }
    }
}
