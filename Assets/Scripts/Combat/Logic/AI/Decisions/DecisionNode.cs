using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions
{
    abstract public class DecisionNode
    {
        protected DecisionNode parent;
        protected DecisionNode next;

        public bool IsFirst()
        {
            return parent == null;
        }

        public bool IsLast()
        {
            return next == null;
        }

        public void Next(DecisionNode node)
        {
            if (next == null)
            {
                next = node;
                next.parent = this;
            }
        }

        abstract public Queue<CombatAction> Traverse();
    }
}