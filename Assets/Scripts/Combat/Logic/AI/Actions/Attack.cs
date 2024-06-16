using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.Systems;
using UnityEngine;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class Attack : CombatDamageAction, IAttackAction
    {
        private UnitGroup pUnit, oUnit;
        private bool isCompleted;

        public Attack(UnitGroup pUnit, UnitGroup oUnit)
        {
            this.pUnit = pUnit;
            this.oUnit = oUnit;
            isCompleted = false;
        }

        public override void Execute()
        {
            if (isCompleted)
            {
                return;
            }

            damageDealt = DamageSystem.Instance().CalculateDamage(pUnit, oUnit, true);

            if (pUnit.Unit.Ability == SpecialAbility.CritDamage && Random.value < 0.3)
            {
                critMultiplier = Random.Range(MIN_CRIT_MULTIPLIER, MAX_CRIT_MULTIPLIER + 1);
                damageDealt *= critMultiplier;
                isCritical = true;
            }

            pUnit.CurrentMoves = 0;
            oUnit.ApplyDamage(damageDealt);

            if (pUnit.Unit.Ability == SpecialAbility.Absorb)
            {
                pUnit.AddHP(damageDealt);
            }

            if (pUnit.Unit.Ability == SpecialAbility.Heal)
            {
                pUnit.RestoreHP(damageDealt);
            }

            isCompleted = true;
        }

        public override bool IsCompleted()
        {
            return isCompleted;
        }

        public UnitGroup Defender()
        {
            return oUnit;
        }

        public UnitGroup Attacker()
        {
            return pUnit;
        }

        public override string Message()
        {
            var message = $"Attack {pUnit.Message()} -> {oUnit.Message()} for {damageDealt} damage. ";

            if (isCritical)
            {
                message += $"CRIT({critMultiplier}x)!!!";
            }

            return message;
        }
    }
}