using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Combat
{
    public class ExorcismSpell : PowerEffect
    {
        private const int POWER_MULTIPLIER = 50;

        private readonly CombatSpellTarget target;

        public ExorcismSpell(CombatSpellTarget target)
        {
            this.target = target;
        }

        public override void Cast()
        {
            if (target == null || !target.Target.IsUndead())
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