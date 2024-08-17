using NetworkShared;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CellGenerator : MonoBehaviour
{
    [Inject] GameManager gameManager;
    [Inject] SceneCell.Factory factory;

    [SerializeField] SceneCell cell;

    List<SceneCell> cells;
    float zPos = 100;
    int maxCount = 3;

    void Awake()
    {
        cells = new();
        OnMarkCellHandler.OnMarkCellEvent += MarkCell;
        OnNewRoundHandler.OnNewRound += SetCellsToDefault;
        InstantiateCells();
    }

    void OnDestroy()
    {
        OnMarkCellHandler.OnMarkCellEvent -= MarkCell;
        OnNewRoundHandler.OnNewRound -= SetCellsToDefault;
    }

    public SceneCell FindCell(Cell cell) => cells.Where(x => x.Row == cell.X).Where(y => y.Column == cell.Y).FirstOrDefault();

    void InstantiateCells()
    {
        int coef = maxCount / 2;
        int minValue = -coef;
        int maxValue = coef;

        for (int i = minValue; i <= maxValue; i++)
        {
            for (int j = minValue; j <= maxValue; j++)
            {
                InstantiateCell(i, j);
            }
        }
    }

    void InstantiateCell(int row, int column)
    {
        SceneCell instCell = factory.Create();
        Vector3 pos = new Vector3(row, column, zPos);
        instCell.transform.position = pos;
        instCell.SetPosition(row, column,maxCount);
        cells.Add(instCell);
    }

    void MarkCell(OnMarkCell req)
    {
        SceneCell cell = cells.Where(x => x.Row == req.Cell.X).Where(y => y.Column == req.Cell.Y).FirstOrDefault();
        MarkType playerType = gameManager.ActiveGame.GetMarkType(req.Actor);
        cell.FillCell(playerType);
    }

    void SetCellsToDefault()
    {
        foreach(var cell in cells)
        {
            cell.SetCellToDefault();
        }
    }
}
