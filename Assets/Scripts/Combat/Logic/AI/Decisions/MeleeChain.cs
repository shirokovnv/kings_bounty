using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.AI.Decisions.Melee;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat.Logic.AI.Decisions
{
    public class MeleeChain : DecisionChain
    {
        DecisionNode root;
        DecisionContext context;

        public MeleeChain(DecisionContext context)
        {
            this.context = context;
        }

        public override Queue<CombatAction> Traverse()
        {
            root = new PathsToOpponents(context);
            return root.Traverse();
        }
    }
}