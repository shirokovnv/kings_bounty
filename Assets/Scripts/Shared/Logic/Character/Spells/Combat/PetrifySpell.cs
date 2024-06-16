using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Combat
{
    public class PetrifySpell : ICastable
    {
        private readonly CombatSpellTarget target;

        public PetrifySpell(CombatSpellTarget target)
        {
            this.target = target;
        }

        public void Cast()
        {
            if (target == null)
            {
                return;
            }

            target.Target.IsPetrified = true;
        }
    }
}