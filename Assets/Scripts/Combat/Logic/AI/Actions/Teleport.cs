using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class Teleport : CombatAction
    {
        private readonly CombatSpellTarget target;

        public Teleport(CombatSpellTarget target)
        {
            this.target = target;
        }

        public override void Execute()
        {
            Spellbook.Instance().UseSpell(target.Source, target);
        }

        public override bool IsCompleted()
        {
            return true;
        }

        public override string Message()
        {
            return $"Teleporting {target.Target.Message()} to {target.X}, {target.Y}";
        }
    }
}