using Assets.Resources.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Combat
{
    abstract public class PowerEffect : ICastable
    {
        public abstract void Cast();
        abstract public int Effect();

        public int CalculateSpellEffect(UnitScriptableObject target)
        {
            return Mathf.FloorToInt(Effect() * (1.0f - target.Resistances[(int)DamageSource.Arcane]));
        }
    }
}