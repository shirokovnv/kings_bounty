using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Combat
{
    public class ResurrectSpell : PowerEffect
    {
        private const int POWER_MULTIPLIER = 20;

        private readonly CombatSpellTarget target;

        public ResurrectSpell(CombatSpellTarget target)
        {
            this.target = target;
        }

        public override void Cast()
        {
            if (target == null)
            {
                return;
            }

            target.Target.RestoreHP(CalculateSpellEffect(target.Target.Unit));
        }

        public override int Effect()
        {
            return PlayerStats.Instance().GetSpellPower() * POWER_MULTIPLIER;
        }
    }
}