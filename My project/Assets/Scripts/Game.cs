using NetworkShared;
using System;

public class Game 
{
    private Guid id;
    private string xUser;
    private string oUser;
    private string currentUser;

    public Guid Id => id;
    public string XUser => xUser;
    public string OUser => oUser;   
    public string CurrentUser => currentUser;

    public Game(Guid id, string xUser, string oUser)
    {
        this.id = id;
        this.xUser = xUser;
        this.oUser = oUser;
        currentUser = xUser;
    }

    public void SwitchActivePlayer() => currentUser = GetOpponent(currentUser);

    public MarkType GetMarkType(string userName)
    {
        if (userName == xUser) return MarkType.X;
        else return MarkType.O;
    }

    string GetOpponent(string userName)
    {
        if (userName == xUser) return OUser;

        return XUser;
    }
}
