using Assets.Scripts.Shared.Data.State;
using System.Collections.Generic;

namespace Assets.Scripts.Adventure.Logic.Interactors
{
    public class DialogInteractionHandler
    {
        private readonly List<IDialogInteractor> interactors;

        public DialogInteractionHandler(List<IDialogInteractor> interactors)
        {
            this.interactors = interactors;
        }

        public void Handle(GameState state)
        {
            foreach (var interactor in interactors)
            {
                interactor.Interact(state);
            }
        }
    }
}