using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Combat
{
    public class CloneSpell : PowerEffect
    {
        private const int POWER_MULTIPLIER = 10;

        private readonly CombatSpellTarget target;

        public CloneSpell(CombatSpellTarget target)
        {
            this.target = target;
        }

        public override void Cast()
        {
            if (target == null)
            {
                return;
            }

            target.Target.AddHP(CalculateSpellEffect(target.Target.Unit));
        }

        public override int Effect()
        {
            return PlayerStats.Instance().GetSpellPower() * POWER_MULTIPLIER;
        }
    }
}