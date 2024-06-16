using Assets.Scripts.Shared.Data.State.Adventure;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Objects
{
    abstract public class BaseObjectInteractor : IInteractor<InteractingWithObject>
    {
        abstract public void OnEntering(InteractingWithObject state);

        abstract public void OnVisiting(InteractingWithObject state);

        abstract public void OnExiting(InteractingWithObject state);

        public void Interact(InteractingWithObject state)
        {
            switch (state.Stage)
            {
                case InteractingWithObject.InteractingStage.entering: OnEntering(state); break;
                case InteractingWithObject.InteractingStage.visiting: OnVisiting(state); break;
                case InteractingWithObject.InteractingStage.exiting: OnExiting(state); break;
            }
        }
    }
}