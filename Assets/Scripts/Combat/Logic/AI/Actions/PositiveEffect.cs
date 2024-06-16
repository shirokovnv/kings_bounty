using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class PositiveEffect : CombatAction
    {
        private readonly CombatSpellTarget target;
        private int quantityDiff;

        public PositiveEffect(CombatSpellTarget target)
        {
            this.target = target;
        }

        public override void Execute()
        {
            int oldQuantity = target.Target.CurrentQuantity();

            Spellbook.Instance().UseSpell(target.Source, target);

            quantityDiff = target.Target.CurrentQuantity() - oldQuantity;
        }

        public override bool IsCompleted()
        {
            return true;
        }

        public override string Message()
        {
            if (target.Source.Name == "Resurrect")
            {
                return $"Resurrected {quantityDiff} {target.Target.Message()}";
            }

            if (target.Source.Name == "Clone")
            {
                return $"Cloned {quantityDiff} {target.Target.Message()}";
            }

            return string.Empty;
        }
    }
}