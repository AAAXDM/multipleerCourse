using UnityEngine;
using NetworkShared;
using Zenject;

public class SceneCell : MonoBehaviour
{
    [Inject] NetworkingClient client;

    [SerializeField] GameObject x;
    [SerializeField] GameObject o;

    bool isFilled;
    byte row;
    byte column;

    public bool IsFilled => isFilled;
    public byte Row => row;
    public byte Column => column;

    public void SetPosition(int row, int column, int maxCount)
    {
        int coef = maxCount / 2;
        int intRow = row + coef;
        int intColumn = column + coef;
        bool canRow = intRow < byte.MaxValue && intRow >= 0;
        bool canColumn = intColumn < byte.MaxValue && intColumn >= 0;
        if(canRow && canColumn)
        {
            this.row = (byte)intRow;
            this.column = (byte)intColumn;
        }    
    }

    public void SendMessageToServer()
    {
        Cell cell = new Cell()
        {
            X = row,
            Y = column,
        };
        var msg = new MarkCellRequest()
        {
            Cell = cell 
        };
        client.SendOnServer(msg);
    }

    public void FillCell(MarkType markType)
    {
        if (markType == MarkType.X) x.SetActive(true);
        else o.SetActive(true);

        isFilled = true;
    }

    public class Factory : PlaceholderFactory<SceneCell>
    {
    }
}
