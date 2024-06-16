using Assets.Scripts.Combat.Logic.AI.Actions.Base;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class Wait : CombatAction
    {
        private readonly UnitGroup group;

        public Wait(UnitGroup group)
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
            return $"{group.Message()} waits";
        }
    }
}