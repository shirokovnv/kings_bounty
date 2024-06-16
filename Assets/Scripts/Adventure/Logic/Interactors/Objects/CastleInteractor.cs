using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Shared.Data.State.Adventure;
using System.Collections.Generic;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects
{
    public class CastleInteractor : BaseObjectInteractor
    {
        private static readonly Dictionary<CastleOwner, BaseObjectInteractor> interactors = new()
    {
        { CastleOwner.opponent, new OpponentCastleInteractor() },
        { CastleOwner.king, new KingsCastleInteractor() },
        { CastleOwner.player, new PlayerCastleInteractor() },
    };

        public override void OnEntering(InteractingWithObject state)
        {
            var castle = state.Obj as Castle;

            castle.SetVisited(true);

            var interactor = interactors[castle.GetOwner()];
            interactor.OnEntering(state);
        }

        public override void OnExiting(InteractingWithObject state)
        {
            var castle = state.Obj as Castle;

            var interactor = interactors[castle.GetOwner()];
            interactor.OnExiting(state);
        }

        public override void OnVisiting(InteractingWithObject state)
        {
            var castle = state.Obj as Castle;

            var interactor = interactors[castle.GetOwner()];
            interactor.OnVisiting(state);
        }
    }
}