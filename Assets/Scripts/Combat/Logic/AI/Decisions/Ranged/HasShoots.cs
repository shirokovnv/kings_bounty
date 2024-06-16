using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.AI.Decisions.Melee;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Ranged
{
    public class HasShoots : DecisionNode
    {
        DecisionContext data;

        public HasShoots(DecisionContext data)
        {
            this.data = data;
        }

        public bool CheckCondition()
        {
            return data.PGroup.CurrentShoots > 0;
        }

        public override Queue<CombatAction> Traverse()
        {
            DecisionNode next = CheckCondition()
                ? new IsBlocked(data)
                : new PathsToOpponents(data);

            Next(next);

            return next.Traverse();
        }
    }
}