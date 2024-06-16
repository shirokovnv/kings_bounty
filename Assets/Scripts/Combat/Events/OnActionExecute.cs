using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using System;

namespace Assets.Scripts.Combat.Events
{
    public class OnActionExecute : EventArgs
    {
        public CombatAction CombatAction;
    }
}