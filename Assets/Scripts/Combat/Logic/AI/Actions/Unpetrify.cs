using Assets.Scripts.Combat.Logic.AI.Actions.Base;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class Unpetrify : CombatAction
    {
        private readonly UnitGroup target;

        public Unpetrify(UnitGroup target)
        {
            this.target = target;
        }

        public override void Execute()
        {
            target.IsPetrified = false;
            target.CurrentMoves = 0;
        }

        public override bool IsCompleted()
        {
            return true;
        }

        public override string Message()
        {
            return $"{target.Message()} is petrified.";
        }
    }
}