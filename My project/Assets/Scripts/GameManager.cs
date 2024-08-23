using System;

public class GameManager 
{
    Game activeGame;
    string myUserName;
    bool canPlay;

    public Game ActiveGame => activeGame;
    public string MyUserName => myUserName;
    public bool CanPlay => canPlay;
    public bool IsMyTurn
    {
        get
        { 
            if(activeGame != null && activeGame.CurrentUser != myUserName) return false;
            else return true;
        }
    }

    public void RegisterGame(Guid id, string xUser, string oUser)
    {
        activeGame = new Game(id, xUser, oUser);
        canPlay = true;
    }

    public void SetUserName(string username) => myUserName = username;

    public void DeleteActiveGame() => activeGame = null;

    public void SetCanPlay(bool value) => canPlay = value;
}
