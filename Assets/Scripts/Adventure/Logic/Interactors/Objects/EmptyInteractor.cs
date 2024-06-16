using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Objects
{
    public class EmptyInteractor : BaseObjectInteractor
    {
        public override void OnEntering(InteractingWithObject state)
        {
            GameStateManager.Instance().SetState(
                new Adventuring()
                );
        }

        public override void OnExiting(InteractingWithObject state)
        {
            // do nothing
        }

        public override void OnVisiting(InteractingWithObject state)
        {
            // do nothing
        }
    }
}