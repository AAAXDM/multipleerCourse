using System;
using Zenject;
using UnityEngine;
using NetworkShared;

public class GameManager : IInitializable, IDisposable
{
    [Inject] NetworkingClient server;
    Game activeGame;
    string myUserName;

    public Game ActiveGame => activeGame;
    public string MyUserName => myUserName;
    public bool IsMyTurn
    {
        get
        {
            if(activeGame.CurrentUser != myUserName) return false;
            else return true;
        }
    }

    public void Initialize() => Application.quitting += Quit;

    public void Dispose() => Application.quitting -= Quit;

    public void RegisterGame(Guid id, string xUser,string oUser) => activeGame = new Game(id, xUser, oUser);

    public void SetUserName(string username) => myUserName = username;

    public void DeleteActiveGame() => activeGame = null;

    void Quit()
    {
        if (activeGame != null)
        {
            var req = new FinishGameRequest()
            {
                IsFinished = true
            };

            server.SendOnServer(req);
        }
    }
}
