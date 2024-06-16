using Assets.Scripts.Shared.Data.State;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors
{
    public interface IInteractor<T> where T : GameState
    {
        public void Interact(T state);
    }
}