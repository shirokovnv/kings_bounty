using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;
using System;

namespace Assets.Scripts.Combat.Events
{
    public class OnChooseSpell : EventArgs
    {
        public CombatSpellTarget SpellTarget;
    }
}