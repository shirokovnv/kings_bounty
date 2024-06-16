using Assets.Scripts.Combat.Logic.AI.Actions;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Melee
{
    public class MoveToOpponent : DecisionNode
    {
        DecisionContext data;
        List<PathNode> path;
        int enemyIndex;

        public MoveToOpponent(DecisionContext data, List<PathNode> path, int enemyIndex)
        {
            this.data = data;
            this.path = path;
            this.enemyIndex = enemyIndex;

            if (enemyIndex < 0 || enemyIndex > data.OGroups.Count)
            {
                throw new System.Exception("Incorrect enemy index.");
            }
        }

        public override Queue<CombatAction> Traverse()
        {
            // leaf
            var queue = new Queue<CombatAction>();
            queue.Enqueue(new MoveToPosition(data.PGroup, path));

            return queue;
        }
    }
}