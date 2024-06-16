using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions
{
    abstract public class DecisionChain
    {
        abstract public Queue<CombatAction> Traverse();
    }
}