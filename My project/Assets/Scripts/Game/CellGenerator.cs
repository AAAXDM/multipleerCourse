
using UnityEngine;

public class CellGenerator : MonoBehaviour
{
    [SerializeField] Cell cell;
    int maxCount = 3;

    void Awake()
    {
        int coef = maxCount / 2;
        int minValue = -coef;
        int maxValue = coef;

        for(int i = minValue; i < maxValue; i++)
        {
            for(int j = minValue; j < maxValue; j++)
            {
                InstantiateCell(i, j);
            }
        }
    }

    void InstantiateCell(int row, int column)
    {
        Cell instCell = Instantiate(cell);
        Vector3 pos = new Vector3(row, column);
        instCell.transform.position = pos;
        instCell.SetPosition(row, column,maxCount);
    }
}
