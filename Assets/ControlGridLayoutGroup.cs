using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlGridLayoutGroup : MonoBehaviour
{
    [SerializeField] GridLayoutGroup layoutGroup;

    private Vector2 StartCellSize;
    private Vector2 MinCellSize = new Vector2(125, 156.25f);
    private Vector2 MaxCellSize = new Vector2(140, 200f);
    public float coef = 0.25f;
    private void Start()
    {
        StartCellSize = layoutGroup.cellSize;
    }

    private void Update()
    {
        int childCount = transform.childCount;

        var calculateCellSize = StartCellSize / (coef * (childCount));

        if (calculateCellSize.x > MinCellSize.x && calculateCellSize.y > MinCellSize.y && calculateCellSize.x < MaxCellSize.x && calculateCellSize.y < MaxCellSize.y)
        {
            layoutGroup.cellSize = calculateCellSize;
        }
        else if (calculateCellSize.x > MaxCellSize.x && calculateCellSize.y > MaxCellSize.y)
        {
            layoutGroup.cellSize = MaxCellSize;
        }
        else
        {
            layoutGroup.cellSize = MinCellSize;
        }
    }
}
