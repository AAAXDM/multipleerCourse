using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] Color xColor;
    [SerializeField] Color oColor;
    LineRenderer lineRenderer;
    float transformCoef = 0.5f;
    int positionsCount = 15;
    int delayTime = 30;
    List<Vector3> positions;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        positions = new();
    }

    public async UniTask DrawLine(SceneCell startCell, SceneCell endCell, bool isX)
    {
        lineRenderer.startColor = isX ? xColor : oColor;
        lineRenderer.endColor = isX ? xColor : oColor;
        lineRenderer.positionCount = 0;
        positions.Clear();
        await CalculatePositions(startCell.transform.position, endCell.transform.position);
    }

    async UniTask CalculatePositions(Vector3 start, Vector3 end)
    {
        float deltaX = 0;
        float deltaY = 0;
        Vector3 startPoint = start;
        bool xEquals = start.x == end.x;

        if (start.y != end.y)
        {
           if(start.y > end.y)
           {
                deltaY = ((end.y - transformCoef) - (start.y + transformCoef)) / positionsCount;
           }
           else
           {
              deltaY = Mathf.Abs((start.y - transformCoef) - (end.y + transformCoef)) / positionsCount;
           }

           if(xEquals)
           {
                startPoint = new Vector3(start.x, end.y + transformCoef, start.z);
           }
           else
           {
                if (deltaY > 0)
                {
                    startPoint = new Vector3(start.x, start.y - transformCoef, start.z);
                }
                else
                {
                    startPoint = new Vector3(start.x, start.y + transformCoef, start.z);
                }
                deltaY = -deltaY;
           }
        }
        if(!xEquals)
        {
           deltaX = Mathf.Abs((start.x  - transformCoef) - (end.x + transformCoef))/ positionsCount;
           startPoint = new Vector3(startPoint.x - transformCoef, startPoint.y, startPoint.z);
        }

         await DrawAsync(startPoint, deltaX, deltaY);
    }

    async UniTask DrawAsync(Vector3 start, float deltaX, float deltaY)
    {
        Vector3 pos;
        for(int i = 0;i <= positionsCount;i++) 
        {
            lineRenderer.positionCount++;
            pos = new Vector3(start.x + i * deltaX, start.y - i * deltaY, start.z);
            lineRenderer.SetPosition(i, pos);
            await UniTask.Delay(delayTime, cancellationToken: destroyCancellationToken);
        }
    }
}
