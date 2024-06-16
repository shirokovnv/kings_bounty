using Assets.Scripts.Combat.Logic.AI.Actions;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Ranged
{
    public class ShootTarget : DecisionNode
    {
        DecisionContext data;

        public ShootTarget(DecisionContext data)
        {
            this.data = data;
        }

        public override Queue<CombatAction> Traverse()
        {
            UnitGroup target = TargetSelector.ChooseRandomTarget(data.OGroups);

            var queue = new Queue<CombatAction>();
            queue.Enqueue(new Preparation());
            queue.Enqueue(new Shoot(data.PGroup, target));

            return queue;
        }
    }
}