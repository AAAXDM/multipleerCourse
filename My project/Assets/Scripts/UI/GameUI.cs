using Cysharp.Threading.Tasks;
using NetworkShared;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameUI : MonoBehaviour
{
    [Inject] GameManager gameManager;
    [Inject] CellGenerator cellGenerator;
    [Inject] NetworkingClient networkingClient;

    [SerializeField] GameUserUI xUser;
    [SerializeField] GameUserUI oUser;
    [SerializeField] TurnChanger turnChanger;
    [SerializeField] LineDrawer lineDrawer;
    [SerializeField] EndRoundUi endRoundUi;
    [SerializeField] Button giveUpButton;

    bool isPanelActive;

    void Start()
    {
        endRoundUi.gameObject.SetActive(false);
        lineDrawer.gameObject.SetActive(false);
        InitGameUI();
        OnMarkCellHandler.OnMarkCellEvent += MarkCellCallback;
        OnNewRoundHandler.OnNewRound += NewRoundCallBack;
        OnMarkCellHandler.SurrenderEvent += SurrenderCallback;
        FinishGameHandler.OnFinishGame += FinishGameCallback;
        giveUpButton.onClick.AddListener(GiveUp);
    }

    void OnDestroy()
    {
        OnMarkCellHandler.OnMarkCellEvent -= MarkCellCallback;
        OnNewRoundHandler.OnNewRound -= NewRoundCallBack;
        OnMarkCellHandler.SurrenderEvent -= SurrenderCallback;
        FinishGameHandler.OnFinishGame -= FinishGameCallback;
        giveUpButton.onClick.RemoveListener(GiveUp);
    }

    void FinishGameCallback()
    {
        if (!isPanelActive)
        {
            gameManager.DeleteActiveGame();
            ShowEndRoundPanel(StateType.Draw);
        }
    }

    void SurrenderCallback(OnMarkCell req)
    {
        bool isX = gameManager.ActiveGame.XUser == req.Actor;
        ChangeUIAfterWin(req, isX);
    }

    void InitGameUI()
    {
        Game game = gameManager.ActiveGame;
        xUser.SetUsername("[X]" + game.XUser);
        oUser.SetUsername("[O]" + game.OUser);
        ChangeEnemyTurn(gameManager.ActiveGame.CurrentUser);
    }

    void ShowEndRoundPanel(StateType type)
    {
        endRoundUi.Init(type);
        endRoundUi.gameObject.SetActive(true);
    }

    void IncreaseScore(bool isX)
    {
        if(isX)
        {
            gameManager.ActiveGame.IncreaseXScore();
            xUser.SetScore(gameManager.ActiveGame.XScore);
        }
        else
        {
            gameManager.ActiveGame.IncreaseOScore();
            oUser.SetScore(gameManager.ActiveGame.OScore);
        }
    }

    void NewRoundCallBack()
    {
        isPanelActive = false;
        endRoundUi.gameObject.SetActive(false);
        lineDrawer.gameObject.SetActive(false);
        ChangeEnemyTurn(gameManager.ActiveGame.CurrentUser);
    }

    void ChangeUIAfterWin(OnMarkCell req, bool isX)
    {
        IncreaseScore(isX);
        if (req.Actor == gameManager.MyUserName)
            ShowEndRoundPanel(StateType.Win);
        else
            ShowEndRoundPanel(StateType.Lose);
    }

    void GiveUp()
    {
        MarkCellRequest response = new()
        {
            IsSurrendering = true
        };
        networkingClient.SendOnServer(response);
    }

    void MarkCellCallback(OnMarkCell req) => MarkCellAsync(req).Forget();

    void ChangeEnemyTurn(string userName) => turnChanger.ChangePlayerTurn(userName);

    async UniTask MarkCellAsync(OnMarkCell req)
    {
        switch (req.Outcome)
        {
            case MarkOutcome.None:
                ChangeEnemyTurn(gameManager.ActiveGame.CurrentUser);
                break;
            case MarkOutcome.Win:
                isPanelActive = true;
                bool isX = gameManager.ActiveGame.XUser == req.Actor;
                await DrawWinLine(req.Result.StartCell, req.Result.EndCell, isX);
                ChangeUIAfterWin(req, isX);
                break;
            case MarkOutcome.Draw:
                isPanelActive = true;
                ShowEndRoundPanel(StateType.Draw);
                break;
        }
    }

    async UniTask DrawWinLine(Cell startCell, Cell endCell, bool isX)
    {
        SceneCell start = cellGenerator.FindCell(startCell);
        SceneCell end = cellGenerator.FindCell(endCell);
        lineDrawer.gameObject.SetActive(true);
        await lineDrawer.DrawLine(start, end, isX);
    }
}
