using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Combat
{
    public class FireballSpell : PowerEffect
    {
        private const int POWER_MULTIPLIER = 25;

        private readonly CombatSpellTarget target;

        public FireballSpell(CombatSpellTarget target)
        {
            this.target = target;
        }

        public override void Cast()
        {
            if (target == null)
            {
                return;
            }

            target.Target.ApplyDamage(CalculateSpellEffect(target.Target.Unit));
        }

        public override int Effect()
        {
            return PlayerStats.Instance().GetSpellPower() * POWER_MULTIPLIER;
        }
    }
}