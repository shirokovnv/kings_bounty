using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Actions.Base
{
    public class CombatActionWrapper
    {
        public UnitGroup Unit { get; }
        public Queue<CombatAction> Actions { get; }

        public CombatActionWrapper(UnitGroup unit, Queue<CombatAction> actions)
        {
            Unit = unit;
            Actions = actions;
        }
    }
}