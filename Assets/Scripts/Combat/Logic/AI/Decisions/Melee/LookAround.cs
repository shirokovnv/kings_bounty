using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Melee
{
    public class LookAround : DecisionNode
    {
        DecisionContext data;
        List<List<PathNode>> pathsToEnemies;
        List<List<PathNode>> nearestPaths;

        public LookAround(DecisionContext data, List<List<PathNode>> pathsToEnemies)
        {
            this.data = data;
            this.pathsToEnemies = pathsToEnemies;
            nearestPaths = new List<List<PathNode>>();

            Debug.Log("Look around " + pathsToEnemies.Count);
        }

        public bool CheckCondition()
        {
            foreach (var path in pathsToEnemies)
            {
                if (path.Count - 1 <= data.PGroup.CurrentMoves)
                {
                    nearestPaths.Add(path);
                }
            }

            return nearestPaths.Count > 0;
        }

        public override Queue<CombatAction> Traverse()
        {
            bool hasEnemiesNear = CheckCondition();
            var pathsToUse = hasEnemiesNear ? nearestPaths : pathsToEnemies;
            var path = TargetSelector.ChooseRandomPath(pathsToUse);

            PathNode targetNode = path.Last();

            int enemyIndex = -1;
            for (int i = 0; i < data.OGroups.Count; i++)
            {
                UnitGroup group = data.OGroups[i];
                if (group.X == targetNode.X && group.Y == targetNode.Y)
                {
                    enemyIndex = i; break;
                }
            }

            DecisionNode next = hasEnemiesNear
                ? new AttackOpponent(data, path, enemyIndex)
                : new MoveToOpponent(data, path, enemyIndex);

            Next(next);

            return next.Traverse();
        }
    }
}