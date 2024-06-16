using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Combat
{
    public class TeleportSpell : ICastable
    {
        private readonly CombatSpellTarget target;

        public TeleportSpell(CombatSpellTarget target)
        {
            this.target = target;
        }

        public void Cast()
        {
            if (target == null)
            {
                return;
            }

            target.Target.X = target.X;
            target.Target.Y = target.Y;
        }
    }
}