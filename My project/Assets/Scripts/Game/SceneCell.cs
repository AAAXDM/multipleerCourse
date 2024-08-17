using UnityEngine;
using NetworkShared;
using Zenject;

public class SceneCell : MonoBehaviour
{
    [Inject] NetworkingClient client;

    [SerializeField] GameObject x;
    [SerializeField] GameObject o;

    Vector3 scale = new Vector3(0.7f, 0.7f, 0.7f);
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
            Cell = cell,
            IsSurrendering = false
        };
        client.SendOnServer(msg);
    }

    public void FillCell(MarkType markType)
    {
        isFilled = true;
        if (markType == MarkType.X) ActivateCell(x);
        else ActivateCell(o);      
    }

    public void SetCellToDefault()
    {
        if (isFilled)
        {
            isFilled = false;
            x.SetActive(false);
            o.SetActive(false);
        }
    }

    void ActivateCell(GameObject obj)
    {
        obj.transform.localScale = scale;
        obj.SetActive(true);
        LeanTween.scale(obj, Vector3.one,0.3f).setEase(LeanTweenType.easeInOutBounce);
    }

    public class Factory : PlaceholderFactory<SceneCell>
    {
    }
}
