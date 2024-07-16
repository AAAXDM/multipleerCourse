using System;
using Zenject;

public class GameManager : IInitializable
{
    Game activeGame;
    bool isInputEnabled;

    public Game ActiveGame => activeGame;

    public void Initialize()
    {
        
    }

    public void RegisterGame(Guid id, string xUser,string oUser)
    {
        activeGame = new Game(id, xUser, oUser);
        isInputEnabled = true;
    }
}
