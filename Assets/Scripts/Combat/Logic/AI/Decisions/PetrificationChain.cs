using Assets.Scripts.Combat.Logic.AI.Actions;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions
{
    public class PetrificationChain : DecisionChain
    {
        private readonly UnitGroup target;

        public PetrificationChain(UnitGroup target)
        {
            this.target = target;
        }

        public override Queue<CombatAction> Traverse()
        {
            var actions = new Queue<CombatAction>();
            actions.Enqueue(new Unpetrify(target));

            return actions;
        }
    }
}