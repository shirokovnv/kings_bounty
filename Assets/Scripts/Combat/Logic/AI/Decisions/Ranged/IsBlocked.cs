using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Ranged
{
    public class IsBlocked : DecisionNode
    {
        DecisionContext data;

        public IsBlocked(DecisionContext data)
        {
            this.data = data;
        }

        public bool CheckCondition()
        {
            return data.OGroups.Where((group) =>
            {
                return Mathf.Abs(group.X - data.PGroup.X) <= 1 &&
                        Mathf.Abs(group.Y - data.PGroup.Y) <= 1;
            }).Count() > 0;
        }

        public override Queue<CombatAction> Traverse()
        {
            DecisionNode next = CheckCondition()
                ? new HasFreeSpace(data)
                : new HasTargets(data);

            Next(next);

            return next.Traverse();
        }
    }
}