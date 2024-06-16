using Assets.Scripts.Combat;
using System.Collections.Generic;
using UnityEngine;

public class CastleBattleField : BattleField
{
    private readonly static List<Vector2Int> oPositions = new()
    {
        new Vector2Int(1, 4),
        new Vector2Int(2, 4),
        new Vector2Int(3, 4),
        new Vector2Int(4, 4),
        new Vector2Int(2, 3),
        new Vector2Int(3, 3),
    };

    private readonly static List<Vector2Int> pPositions = new()
    {
        new Vector2Int(2, 0),
        new Vector2Int(3, 0),
        new Vector2Int(2, 1),
        new Vector2Int(3, 1),
        new Vector2Int(1, 1),
        new Vector2Int(4, 1),
    };


    private readonly static List<Vector2Int> gatePositions = new()
    {
        new Vector2Int(2, 0),
        new Vector2Int(3, 0),
    };

    public CastleBattleField(
        int width,
        int height,
        List<UnitGroup> pList,
        List<UnitGroup> oList) : base(width, height, pList, oList)
    {
    }

    protected override void FillTheGrid()
    {
        // bottom border
        for (int i = 0; i < Width; i++)
        {
            Vector2Int position = new(i, 0);

            if (!gatePositions.Contains(position))
            {
                Grid.GetValue(i, 0).SetObstacle(true);
            }
        }

        // left and right border
        for (int i = 1; i < Height; i++)
        {
            Grid.GetValue(0, i).SetObstacle(true);
            Grid.GetValue(Width - 1, i).SetObstacle(true);
        }
    }

    protected override void PrepareSquadPositions()
    {
        for (int i = 0; i < PList.Count; i++)
        {
            PList[i].X = pPositions[i].x;
            PList[i].Y = pPositions[i].y;
        }

        for (int i = 0; i < OList.Count; i++)
        {
            OList[i].X = oPositions[i].x;
            OList[i].Y = oPositions[i].y;
        }
    }
}
