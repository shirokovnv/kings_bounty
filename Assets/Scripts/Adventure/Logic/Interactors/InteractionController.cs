using Assets.Scripts.Adventure.Logic.Interactors.Dialogs;
using Assets.Scripts.Adventure.Logic.Interactors.Objects.Factory;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Data.State;
using Assets.Scripts.Shared.Data.State.Adventure;
using System.Collections.Generic;
using Assets.Scripts.Adventure.Logic.Continents;

namespace Assets.Scripts.Adventure.Logic.Interactors
{
    public class InteractionController : IInteractor<GameState>
    {
        private readonly Continent continent;

        private static readonly List<IDialogInteractor> dialogInteractors = new()
        {
                new GreetingsInteractor(),
                new InventoryInteractor(),
                new SpellBookInteractor(),
                new CastSpellInteractor(),
                new SquadsInteractor(),
                new ContractInteractor(),
                new ContinentInteractor(),
                new DismissalInteractor(),
                new MapInteractor(),
                new SpoilsOfWarInteractor(),
                new DefeatInteractor(),
                new WeekEndInteractor(),
                new PuzzleMapInteractor(),
                new ControlsInteractor(),
                new SearchInteractor(),
        };

        public InteractionController(Continent continent)
        {
            this.continent = continent;
        }

        public void Interact(GameState state)
        {
            var interactionHandler = new DialogInteractionHandler(dialogInteractors);
            interactionHandler.Handle(GameStateManager.Instance().GetState());

            // interact with world
            if (state is Adventuring)
            {
                int x = Player.Instance().X;
                int y = Player.Instance().Y;
                bool isNavigating = Player.Instance().IsNavigating();

                var worldInteractor = new WorldInteractor(x, y, continent, isNavigating);
                worldInteractor.Interact(GameStateManager.Instance().GetState() as Adventuring);
            }

            // interact with object
            if (state is InteractingWithObject)
            {
                var interactor = ObjectInteractionFactory.CreateInteractor((state as InteractingWithObject).Obj);

                interactor.Interact(state as InteractingWithObject);
            }
        }
    }
}