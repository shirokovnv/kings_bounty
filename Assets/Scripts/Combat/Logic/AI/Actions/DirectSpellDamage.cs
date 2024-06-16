using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Logic.Character.Spells;
using Assets.Scripts.Shared.Logic.Character.Spells.Combat;
using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class DirectSpellDamage : CombatAction, IAttackAction
    {
        private readonly CombatSpellTarget target;
        private int damageDealt;

        public DirectSpellDamage(CombatSpellTarget target)
        {
            this.target = target;
        }

        public UnitGroup Attacker()
        {
            return null;
        }

        public UnitGroup Defender()
        {
            return target.Target;
        }

        public override void Execute()
        {
            var castable = CastableFactory.Create(target.Source, target);

            damageDealt = (castable as PowerEffect).CalculateSpellEffect(target.Target.Unit);

            Spellbook.Instance().UseSpell(target.Source, target);
        }

        public override bool IsCompleted()
        {
            return true;
        }

        public override string Message()
        {
            return $"{target.Source.Name} deals {damageDealt} damage to {target.Target.Message()}";
        }
    }
}