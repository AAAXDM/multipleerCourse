using UnityEngine;
using Zenject;

public class GameUI : MonoBehaviour
{
    [Inject] GameManager gameManager;

    [SerializeField] GameUserUI xUser;
    [SerializeField] GameUserUI oUser;
    [SerializeField] TurnChanger turnChanger;

    void Start()
    {
        InitGameUI();
    }

    void InitGameUI()
    {
        Game game = gameManager.ActiveGame;
        xUser.SetUsername("[X]" + game.XUser);
        oUser.SetUsername("[O]" + game.OUser);
    }
}
