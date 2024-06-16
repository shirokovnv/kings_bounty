using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.AI.Decisions.Flying;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions
{
    public class FlyingChain : DecisionChain
    {
        DecisionContext context;
        DecisionNode root;

        public FlyingChain(DecisionContext context)
        {
            this.context = context;
        }

        public override Queue<CombatAction> Traverse()
        {
            root = new HasFlyingPaths(context);
            return root.Traverse();
        }
    }
}