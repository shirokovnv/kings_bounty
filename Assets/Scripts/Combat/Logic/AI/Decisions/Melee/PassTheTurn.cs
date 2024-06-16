using Assets.Scripts.Combat.Logic.AI.Actions;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Melee
{
    public class PassTheTurn : DecisionNode
    {
        DecisionContext data;

        public PassTheTurn(DecisionContext data)
        {
            this.data = data;
        }

        public override Queue<CombatAction> Traverse()
        {
            var queue = new Queue<CombatAction>();
            queue.Enqueue(new Pass(data.PGroup));

            return queue;
        }
    }
}