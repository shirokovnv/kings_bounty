using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.AI.Decisions.Melee;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Ranged
{
    public class HasTargets : DecisionNode
    {
        DecisionContext data;

        public HasTargets(DecisionContext data)
        {
            this.data = data;
        }

        public bool CheckCondition()
        {
            return data.OGroups.Count > 0;
        }

        public override Queue<CombatAction> Traverse()
        {
            DecisionNode next = CheckCondition()
                ? new ShootTarget(data)
                : new PassTheTurn(data);

            Next(next);

            return next.Traverse();
        }
    }
}