using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Combat;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Targeting
{
    public class CombatSpellTarget : SpellTarget
    {
        public UnitGroup Target;
        public int X, Y;
        public SpellScriptableObject Source;
    }
}