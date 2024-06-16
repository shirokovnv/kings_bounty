using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.AI.Decisions.Ranged;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions
{
    public class RangedChain : DecisionChain
    {
        DecisionContext context;
        DecisionNode root;

        public RangedChain(DecisionContext context)
        {
            this.context = context;
        }

        public override Queue<CombatAction> Traverse()
        {
            root = new HasShoots(context);
            return root.Traverse();
        }
    }
}