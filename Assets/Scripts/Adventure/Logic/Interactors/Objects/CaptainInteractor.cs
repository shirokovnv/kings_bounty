using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Shared.Data.State.Adventure;
using System.Collections.Generic;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects
{
    public class CaptainInteractor : BaseObjectInteractor
    {
        private static readonly Dictionary<bool, BaseCombatableInteractor> interactors = new()
    {
        { true, new LoyalCaptainInteractor() },
        { false, new DisloyalCaptainInteractor() }
    };

        public override void OnEntering(InteractingWithObject state)
        {
            var captain = state.Obj as Captain;

            interactors[captain.IsLoyal()].OnEntering(state);
        }

        public override void OnExiting(InteractingWithObject state)
        {
            var captain = state.Obj as Captain;

            interactors[captain.IsLoyal()].OnExiting(state);
        }

        public override void OnVisiting(InteractingWithObject state)
        {
            var captain = state.Obj as Captain;

            interactors[captain.IsLoyal()].OnVisiting(state);
        }
    }
}