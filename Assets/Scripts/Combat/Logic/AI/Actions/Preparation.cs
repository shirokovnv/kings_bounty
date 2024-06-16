using Assets.Scripts.Combat.Logic.AI.Actions.Base;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class Preparation : CombatAction
    {
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
            return string.Empty;
        }
    }
}