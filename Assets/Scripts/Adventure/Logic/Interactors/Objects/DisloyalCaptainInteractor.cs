using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.UI.BottomPanel;
using Assets.Scripts.Combat.Logic.Systems;
using Assets.Scripts.Loading;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Data.State.Combat;
using Assets.Scripts.Shared.Logic.Character;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects
{
    public class DisloyalCaptainInteractor : BaseCombatableInteractor
    {
        public override void OnEntering(InteractingWithObject state)
        {
            var captain = state.Obj as Captain;

            string title = "Your scouts have sighted: \r\n";
            string text = "\r\n";
            string actionText = "Attack? (Y/N)";

            captain.GetSquads().ForEach(group =>
            {
                text += $"{ApproximateQuantity(group.CurrentQuantity())} {group.Unit.Name} \r\n";
            });

            BottomUIScript.Instance.ShowBottomUI();
            BottomUIScript.Instance.ShowUIMessage(title, text, null, actionText);

            GameStateManager.Instance().SetState(
                new InteractingWithObject
                {
                    Stage = InteractingWithObject.InteractingStage.visiting,
                    Obj = state.Obj,
                }
                );
        }

        public override void OnExiting(InteractingWithObject state)
        {
            BottomUIScript.Instance.HideBottomUI();

            GameStateManager.Instance().SetState(
                new Adventuring()
                );
        }

        public override void OnVisiting(InteractingWithObject state)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.N))
            {
                GameStateManager.Instance().SetState(
                    new InteractingWithObject
                    {
                        Stage = InteractingWithObject.InteractingStage.exiting,
                        Obj = state.Obj,
                    }
                    );
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                MoraleSystem.Instance().CalculateMorale((state.Obj as Captain).GetSquads());
                PlayerSquads.Instance().CalculateModifiers();

                GameStateManager.Instance().SetState(new Combating(
                    state.Obj as Captain,
                    PlayerSquads.Instance().GetSquadsTotalQuantity()
                ));

                (state.Obj as Captain).CalculateSpoilsOfWar();
                (state.Obj as Captain).CalculateLeadership();

                SceneLoader.Load(SceneLoader.Scene.CombatScene);
            }
        }
    }
}