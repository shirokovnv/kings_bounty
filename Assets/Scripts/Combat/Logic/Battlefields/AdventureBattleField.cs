using Assets.Scripts.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureBattleField : BattleField
{
    private const int MIN_OBSTACLES = 4;
    private const int MAX_OBSTACLES = 5;
    private const int MAX_ITERATIONS_COUNT = 100;

    public AdventureBattleField(
        int width, 
        int height, 
        List<UnitGroup> pList, 
        List<UnitGroup> oList
        ) : base(width, height, pList, oList)
    {
    }

    protected override void FillTheGrid()
    {
        GenerateRandomObstacles();
    }

    protected override void PrepareSquadPositions()
    {
        for (int i = 0; i < PList.Count; i++)
        {
            PList[i].X = 0;
            PList[i].Y = Height - i - 1;
        }
        
        for (int j = 0; j < OList.Count; j++)
        {
            OList[j].X = Width - 1;
            OList[j].Y = Height - j - 1;
        }
    }

    protected void GenerateRandomObstacles()
    {
        int maxObstacles = Random.Range(MIN_OBSTACLES, MAX_OBSTACLES + 1);
        int numObstacles = 0;
        int numIterations = 0;

        while (numObstacles < maxObstacles && numIterations < MAX_ITERATIONS_COUNT)
        {
            int x = Random.Range(1, Width - 1);
            int y = Random.Range(1, Height - 1);

            if (Grid.GetValue(x, y).IsObstacle() == false)
            {
                Grid.GetValue(x, y).SetObstacle(true);
                numObstacles++;
            }

            numIterations++;
        }
    }
}
