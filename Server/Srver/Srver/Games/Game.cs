using NetworkShared;

namespace Srver
{
    public class Game
    {
        int maxSize = 3;
        int countToWin = 3;

        public Guid Id { get; set; }
        public User XUser {get;}
        public User OUser {get;}
        public ushort Round { get; set; } 
        public string CurrentUser { get; set; }
        public MarkType[,] Grid { get; private set; }

        public Game(string xUser, string oUser)
        {
            Id = new Guid();
            OUser = new User(oUser);
            XUser = new User(xUser);
            Round = 1;
            CurrentUser = xUser;
            Grid = new MarkType[maxSize, maxSize];
        }

        public MarkResult MarkCell(byte row, byte column)
        {
            MarkType type = GetPlayerType(CurrentUser);
            MarkResult result = new();
            Grid[row, column] = type;
            bool win = CheckWin(row, column, type, out WinResult winResult);
            if (win)
            {
                result.MarkOutcome = MarkOutcome.Win;
                result.WinResult = winResult;
            }
            else
            {
                bool draw = CheckDraw();
                if (draw)
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
            if (winnerType == MarkType.X) XUser.IncreaseWinCount();
            else OUser.IncreaseWinCount();
        }

        public void CreateNewRound()
        {
            CurrentUser = XUser.UserName;
            Grid = new MarkType[maxSize, maxSize];
            XUser.SetWantToRematch(false);
            OUser.SetWantToRematch(false);
            Round++;
        }

        public string GetOpponent(string userName)
        {
            if (userName == XUser.UserName) return OUser.UserName;

            return XUser.UserName;
        }

        public bool CanPlayAgain(string userName)
        {
            MarkType winnerType = GetPlayerType(userName);
            if(winnerType == MarkType.X)
            {
               return WantPlayAgain(XUser, OUser);
            }
            else
            {
              return WantPlayAgain(OUser, XUser);
            }
        }

        bool WantPlayAgain(User firstUser,User secondUser)
        {
            if (!firstUser.WantToRematch)
            {
                firstUser.SetWantToRematch(true);
            }
            if (secondUser.WantToRematch)
            {
                return true;
            }
            
            return false;
        }

        MarkType GetPlayerType(string userName)
        {
            if (userName == XUser.UserName)
            {
                return MarkType.X;
            }
            if (userName == OUser.UserName)
            {
                return MarkType.O;
            }
            return MarkType.None;
        }

        bool CheckWin(byte row, byte col, MarkType type, out WinResult result)
        {
            result = new();
            byte minRow = GetCellCoordinate(row,false);
            byte maxRow = GetCellCoordinate(row, true);
            byte minCol = GetCellCoordinate(col, false);
            byte maxCol = GetCellCoordinate(col, true);

            if (CheckLine(minRow, maxRow, col, row, type, WinLineType.Horizontal))
            {
                result.StartCell = new Cell() { X = minRow, Y = col };
                result.EndCell = new Cell() { X = maxRow, Y = col };
                return true;
            }

            if (CheckLine(minCol, maxCol, row,col, type, WinLineType.Vertical))
            {
                result.StartCell = new Cell() { X = row, Y = minCol };
                result.EndCell = new Cell() { X = row, Y = maxCol };
                return true;
            }

            if(CheckLine(minCol, maxCol, row, col, type, WinLineType.Diagonal))
            {
                result.StartCell = new Cell() { X = minRow, Y = minCol };
                result.EndCell = new Cell() { X = maxRow, Y = maxCol };
                return true;
            }

            if(CheckLine(minCol, maxCol, row, col, type, WinLineType.AntiDiagonal))
            {
                result.StartCell = new Cell() { X = minRow, Y = maxCol };
                result.EndCell = new Cell() { X = maxRow, Y = minCol };
                return true;
            }
           
            return false;
        }

        bool CheckLine(int min, int max, int staticNumber, int middle,MarkType type, WinLineType lineType)
        {
            int j = 0;
            bool horizontal = SetCycleParameters(lineType, out bool vertical);
             
            int x = horizontal ? 0 : staticNumber;
            int y = vertical ?  0 : staticNumber;

            if (middle >= min)
            {
                for (int i = middle; i >= min; i--)
                {
                    if (!IsGridEqualsType(x,y,i,max,horizontal,vertical,lineType,type)) break;
                    j++;
                }
            }

            if (middle + 1 <= max)
            {
                for (int i = middle + 1; i <= max; i++)
                {
                    if (!IsGridEqualsType(x, y, i, max, horizontal, vertical, lineType, type)) break;
                    j++;
                }
            }

            if (j >= countToWin) return true;

            return false;
        }

        bool IsGridEqualsType(int x,int y,int i,int max,bool horizontal, bool vertical,WinLineType lineType, MarkType type)
        {
            if (horizontal)
            {
                x = i;
            }
            if (vertical)
            {
                if (lineType == WinLineType.AntiDiagonal)
                {
                    y = max - i;
                }
                else
                {
                    y = i;
                }
            }
            if (Grid[x, y] != type) return false;
            return true;
        }

        bool SetCycleParameters(WinLineType lineType, out bool vertical)
        {
            bool horizontal = false;
            vertical = false;

            switch (lineType)
            {
                case WinLineType.AntiDiagonal:
                    horizontal = true;
                    vertical = true;
                    break;
                case WinLineType.Diagonal:
                    horizontal = true;
                    vertical = true;
                    break;
                case WinLineType.Horizontal:
                    horizontal = true;
                    break;
                case WinLineType.Vertical:
                    vertical = true;
                    break;
            }

            return horizontal;
        }

        byte GetCellCoordinate(byte pos, bool isMax)
        {
            if(isMax)
            {
                int summ = pos + (countToWin - 1);
                if(summ < (maxSize - 1))
                {
                    return (byte)summ;
                }
                else
                {
                    return (byte)(maxSize - 1);
                }
            }
            else
            {
                int difference = (pos - (countToWin - 1));
                if (difference > 0)
                {
                    return (byte)difference;
                }
                else
                {
                    return 0;
                }
            }
        }

        bool CheckDraw()
        {
            for (int i = 0; i < maxSize; i++)
            {
                for (int j = 0; j < maxSize; j++)
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

    public class User
    {
        string userName;
        ushort winCount;
        bool wantToRematch;

        public string UserName => userName;
        public ushort WinCount => winCount;
        public bool WantToRematch => wantToRematch;

        public User(string userName) => this.userName = userName;

        public void IncreaseWinCount() => winCount++;

        public void SetWantToRematch(bool value) => wantToRematch = value;
    }
}
