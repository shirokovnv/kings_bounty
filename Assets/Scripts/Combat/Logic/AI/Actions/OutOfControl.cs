using Assets.Scripts.Combat.Logic.AI.Actions.Base;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class OutOfControl : CombatAction
    {
        private readonly UnitGroup group;

        public OutOfControl(UnitGroup group)
        {
            this.group = group;
        }

        public override void Execute()
        {
            // do nothing
        }

        public override bool IsCompleted()
        {
            return true;
        }

        public override string Message()
        {
            return $"{group.Message()} is OUT OF CONTROL!!!";
        }
    }
}