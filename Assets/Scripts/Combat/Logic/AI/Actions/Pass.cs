using Assets.Scripts.Combat.Logic.AI.Actions.Base;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class Pass : CombatAction
    {
        private readonly UnitGroup group;
        private bool isCompleted;

        public Pass(UnitGroup group)
        {
            this.group = group;
            isCompleted = false;
        }

        public override void Execute()
        {
            if (isCompleted)
            {
                return;
            }

            group.CurrentMoves = 0;
            isCompleted = true;
        }

        public override bool IsCompleted()
        {
            return isCompleted;
        }

        public override string Message()
        {
            return $"{group.Message()} passes the turn";
        }
    }
}