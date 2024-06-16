using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class NegativeEffect : CombatAction
    {
        private readonly CombatSpellTarget target;

        public NegativeEffect(CombatSpellTarget target)
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
            if (target.Source.Name == "Petrify")
            {
                return $"Petrifying {target.Target.Message()}";
            }

            return string.Empty;
        }
    }
}