using Assets.Scripts.Combat.Logic.AI.Actions;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Melee
{
    public class AttackOpponent : DecisionNode
    {
        DecisionContext data;
        List<PathNode> path;
        int enemyIndex;

        public AttackOpponent(DecisionContext data, List<PathNode> path, int enemyIndex)
        {
            this.data = data;
            this.path = path;
            this.enemyIndex = enemyIndex;
        }

        public override Queue<CombatAction> Traverse()
        {
            var queue = new Queue<CombatAction>();

            queue.Enqueue(new MoveToPosition(data.PGroup, path));

            if (path.Count > 0)
            {
                queue.Enqueue(new Preparation());
            }

            queue.Enqueue(new Attack(data.PGroup, data.OGroups[enemyIndex]));

            if (!data.OGroups[enemyIndex].IsDead() && data.OGroups[enemyIndex].CurrentCounterstrikes > 0)
            {
                queue.Enqueue(new CounterAttack(data.OGroups[enemyIndex], data.PGroup));
            }

            return queue;
        }
    }
}