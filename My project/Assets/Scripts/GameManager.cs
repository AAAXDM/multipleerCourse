using System;

public class GameManager 
{
    Game activeGame;
    string myUserName;

    public Game ActiveGame => activeGame;
    public bool IsMyTurn
    {
        get
        {
            if(activeGame.CurrentUser != myUserName) return false;
            else return true;
        }
    }

    public bool IsX
    {
        get
        {
            if(activeGame.XUser != myUserName) return false;
            else return true;
        }
    }

    public void RegisterGame(Guid id, string xUser,string oUser)
    {
        activeGame = new Game(id, xUser, oUser);
    }

    public void SetUserName(string username) => myUserName = username;
}
