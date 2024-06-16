using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.AI.Decisions.Melee;
using Assets.Scripts.Combat.Logic.AI.Grid;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Ranged
{
    public class HasFreeSpace : DecisionNode
    {
        DecisionContext data;
        List<PathNode> path;

        public HasFreeSpace(DecisionContext data)
        {
            this.data = data;
            path = new List<PathNode>();
        }

        public bool CheckCondition()
        {
            path = AIPathFinder.FindFallbackPathToPosition(data);

            string text = string.Empty;
            foreach (var node in path)
            {
                text += "(" + node.X + "," + node.Y + ") ";
            }

            Debug.Log("Fallback path (" + data.PGroup.Unit.Name + "): " + text);

            return path.Count > 0;
        }

        public override Queue<CombatAction> Traverse()
        {
            DecisionNode next = CheckCondition()
                ? new FallbackAndShoot(data, path)
                : new PathsToOpponents(data);

            Next(next);

            return next.Traverse();
        }
    }
}