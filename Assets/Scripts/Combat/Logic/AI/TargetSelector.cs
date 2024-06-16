using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat.Logic.AI
{
    public static class TargetSelector
    {
        public static List<PathNode> ChooseRandomPath(
            List<List<PathNode>> paths
            )
        {
            if (paths.Count == 0)
            {
                throw new System.Exception("No paths detected.");
            }

            return paths[Random.Range(0, paths.Count)];
        }

        public static UnitGroup ChooseRandomTarget(List<UnitGroup> targets)
        {
            if (targets.Count == 0)
            {
                throw new System.Exception("No targets detected.");
            }

            return targets[Random.Range(0, targets.Count)];
        }
    }
}