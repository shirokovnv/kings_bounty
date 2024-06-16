using Assets.Resources.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Combat.Logic.Systems
{
    [System.Serializable]
    public class DamageSystem
    {
        private const float MAX_LEVEL_DIFF = 5.0f;

        private static DamageSystem instance;

        private DamageSystem() { }

        public static DamageSystem Instance()
        {
            instance ??= new DamageSystem();

            return instance;
        }

        public int CalculateDamage(UnitGroup attacker, UnitGroup defender, bool isMelee)
        {
            int baseDamage = isMelee
                ? attacker.MeleeDamage()
                : attacker.RangedDamage();

            float modCoef = attacker.DamageAmplificationCoefficient - defender.DamageReductionCoefficient;
            float levelCoef = (attacker.Unit.Level - defender.Unit.Level + MAX_LEVEL_DIFF) / MAX_LEVEL_DIFF;
            float moraleCoef = attacker.Morale switch
            {
                MoraleType.Low => 0.5f,
                MoraleType.Normal => 1.0f,
                MoraleType.High => 1.5f,
                MoraleType.OutOfControl => Random.Range(0.5f, 1.0f),
                _ => 1.0f
            };

            DamageSource damageSource = isMelee
                ? attacker.Unit.MeleeDamageSource
                : attacker.Unit.RangeDamageSource;

            float resistCoef = 1.0f - defender.Unit.Resistances[(int)damageSource];

            int totalDamage = Mathf.FloorToInt(baseDamage * (modCoef + moraleCoef) * levelCoef * resistCoef);

            return totalDamage;
        }
    }
}