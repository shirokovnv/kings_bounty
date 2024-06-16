using Assets.Scripts.Combat.Interfaces;

namespace Assets.Scripts.Combat.Logic.AI.Actions.Base
{
    abstract public class CombatAction : IMessageable
    {
        abstract public void Execute();
        abstract public bool IsCompleted();
        abstract public string Message();
    }
}