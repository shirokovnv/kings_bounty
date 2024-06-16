using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.Systems;
using UnityEngine;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class CounterAttack : CombatDamageAction, IAttackAction
    {
        UnitGroup pUnit, oUnit;
        bool isCompleted;

        public CounterAttack(UnitGroup pUnit, UnitGroup oUnit)
        {
            this.pUnit = pUnit;
            this.oUnit = oUnit;
            isCompleted = false;
        }

        public UnitGroup Attacker()
        {
            return pUnit;
        }

        public UnitGroup Defender()
        {
            return oUnit;
        }

        public override void Execute()
        {
            if (isCompleted)
            {
                return;
            }

            isCompleted = true;

            if (pUnit.IsDead() || oUnit.IsDead())
            {
                return;
            }

            if (pUnit.CurrentCounterstrikes > 0)
            {
                damageDealt = DamageSystem.Instance().CalculateDamage(pUnit, oUnit, true);

                if (pUnit.Unit.Ability == SpecialAbility.CritDamage && Random.value < 0.3)
                {
                    isCritical = true;
                    critMultiplier = Random.Range(MIN_CRIT_MULTIPLIER, MAX_CRIT_MULTIPLIER + 1);
                    damageDealt *= critMultiplier;
                }

                oUnit.ApplyDamage(damageDealt);

                if (pUnit.Unit.Ability == SpecialAbility.Absorb)
                {
                    pUnit.AddHP(damageDealt);
                }

                if (pUnit.Unit.Ability == SpecialAbility.Heal)
                {
                    pUnit.RestoreHP(damageDealt);
                }

                pUnit.CurrentCounterstrikes--;
            }
        }

        public override bool IsCompleted()
        {
            return isCompleted;
        }

        public override string Message()
        {
            var message = $"Counter attack {pUnit.Message()} -> {oUnit.Message()} for {damageDealt} damage. ";

            if (isCritical)
            {
                message += $"CRIT({critMultiplier}x)!!!";
            }

            return message;
        }
    }
}