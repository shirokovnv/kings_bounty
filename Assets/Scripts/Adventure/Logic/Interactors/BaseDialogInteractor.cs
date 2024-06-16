using Assets.Scripts.Shared.Data.State;

namespace Assets.Scripts.Adventure.Logic.Interactors
{
    abstract public class BaseDialogInteractor<PrevState, NextState> : IDialogInteractor
        where PrevState : GameState
        where NextState : GameState
    {
        abstract public void WaitForStart();
        abstract public void WaitForFinish();

        public void Interact(GameState state)
        {
            if (state is PrevState)
            {
                WaitForStart();
            }

            if (state is NextState)
            {
                WaitForFinish();
            }
        }
    }
}