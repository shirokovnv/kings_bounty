using Assets.Scripts.Combat.Logic.AI.Actions;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Ranged
{
    public class FallbackAndShoot : DecisionNode
    {
        DecisionContext data;
        List<PathNode> path;

        public FallbackAndShoot(DecisionContext data, List<PathNode> path)
        {
            this.data = data;
            this.path = path;
        }

        public override Queue<CombatAction> Traverse()
        {
            var queue = new Queue<CombatAction>();

            queue.Enqueue(new MoveToPosition(data.PGroup, path, false));

            var targetsQueue = new HasTargets(data).Traverse();

            while (targetsQueue.Count > 0)
            {
                queue.Enqueue(targetsQueue.Dequeue());
            }

            return queue;
        }
    }
}