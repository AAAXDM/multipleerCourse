using UnityEngine;
using NetworkShared;
using Zenject;

public class Cell : MonoBehaviour
{
    [Inject] NetworkingClient client;

    [SerializeField] GameObject X;
    [SerializeField] GameObject O;

    byte row;
    byte column;

    public byte Row => row;
    public byte Column => column;

    public void SetPosition(int row, int column, int maxCount)
    {
        int coef = maxCount / 2;
        int intRow = row + coef;
        int intColumn = column + coef;
        bool canRow = intRow < byte.MaxValue && intRow >= 0;
        bool canColumn = intColumn < byte.MaxValue && column >= 0;
        if(canRow && canColumn)
        {
            this.row = (byte)intRow;
            this.column = (byte)intColumn;
        }
       
    }

    public void Fill(bool isX)
    {
        if (isX) X.SetActive(true);
        else O.SetActive(true);

        var msg = new MarkCellRequest()
        {
            Row = row,
            Col = column
        };
        client.SendOnServer(msg);
    }
}
