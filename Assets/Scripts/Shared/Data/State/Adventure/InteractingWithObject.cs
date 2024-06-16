using Assets.Scripts.Adventure.Logic.Continents.Base;

namespace Assets.Scripts.Shared.Data.State.Adventure
{
    public class InteractingWithObject : InAdventure
    {
        public enum InteractingStage
        {
            entering,
            visiting,
            exiting,
        }

        public InteractingStage Stage;
        public BaseObject Obj;
        public GameState exitingState = new Adventuring();
    }
}