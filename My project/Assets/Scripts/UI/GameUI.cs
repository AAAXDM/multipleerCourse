using NetworkShared;
using UnityEngine;
using Zenject;

public class GameUI : MonoBehaviour
{
    [Inject] GameManager gameManager;
    [Inject] CellGenerator cellGenerator;

    [SerializeField] GameUserUI xUser;
    [SerializeField] GameUserUI oUser;
    [SerializeField] TurnChanger turnChanger;
    [SerializeField] LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer.gameObject.SetActive(false);
        InitGameUI();
        OnMarkCellHandler.OnMarkCellEvent += MarkCellCallback;
    }

    private void OnDestroy() => OnMarkCellHandler.OnMarkCellEvent -= MarkCellCallback;

    void InitGameUI()
    {
        Game game = gameManager.ActiveGame;
        xUser.SetUsername("[X]" + game.XUser);
        oUser.SetUsername("[O]" + game.OUser);
        ChangeEnemyTurn(gameManager.ActiveGame.CurrentUser);
    }

    void MarkCellCallback(OnMarkCell req)
    {
        if (req.Outcome == MarkOutcome.None)
        {
            ChangeEnemyTurn(gameManager.ActiveGame.CurrentUser);
        }
        if(req.Outcome == MarkOutcome.Win)
        {
            DrawWinLine(req.Result.StartCell, req.Result.EndCell);
        }
    }

    void DrawWinLine(Cell startCell, Cell endCell)
    {
        SceneCell start = cellGenerator.FindCell(startCell);
        SceneCell end = cellGenerator.FindCell(endCell);
        lineRenderer.gameObject.SetActive(true);
        lineRenderer.SetPosition(0,start.gameObject.transform.position);
        lineRenderer.SetPosition(1, end.gameObject.transform.position);
    }

    void ChangeEnemyTurn(string userName) => turnChanger.ChangePlayerTurn(userName);
}
