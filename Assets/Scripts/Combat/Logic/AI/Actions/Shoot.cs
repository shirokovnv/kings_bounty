using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.Systems;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class Shoot : CombatAction, IAttackAction
    {
        UnitGroup pUnit;
        UnitGroup oUnit;
        bool isCompleted;
        int damageDealt;

        public Shoot(UnitGroup pUnit, UnitGroup oUnit)
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

            damageDealt = DamageSystem.Instance().CalculateDamage(pUnit, oUnit, false);
            pUnit.CurrentMoves = 0;
            pUnit.CurrentShoots--;
            oUnit.ApplyDamage(damageDealt);

            isCompleted = true;
        }

        public override bool IsCompleted()
        {
            return isCompleted;
        }

        public override string Message()
        {
            return $"Shoot {pUnit.Message()} -> {oUnit.Message()} for {damageDealt} damage.";
        }
    }
}