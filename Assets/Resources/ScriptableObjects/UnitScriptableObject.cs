using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Combat.Logic.Systems;
using UnityEngine;

namespace Assets.Resources.ScriptableObjects
{
    public enum DamageSource
    {
        Hand,
        Pierce,
        Arcane,
        None,
    }

    public enum SpecialAbility
    {
        None,
        Absorb,
        Heal,
        Regenerate,
        CritDamage,
    }

    [CreateAssetMenu(fileName = "unit", menuName = "ScriptableObjects/unit")]
    public class UnitScriptableObject : ScriptableObject
    {
        public string Name;
        public int Level;
        public int MinDamage;
        public int MaxDamage;
        public int MinRangeDamage;
        public int MaxRangeDamage;
        public int HP;
        public int NumCounterstrikes;
        public int NumMoves;
        public int NumShoots;
        public bool CanFly;
        public Dwelling.DwellingType DwellingType;
        public int Population;
        public int Cost;
        public MoraleGroup MoraleGroup;
        public DamageSource MeleeDamageSource;
        public DamageSource RangeDamageSource;
        public float[] Resistances;
        public SpecialAbility Ability;
        public Sprite[] Sprites;

        public int GetScriptID()
        {
            return $"UNIT__{Name}".GetHashCode();
        }
    }

}