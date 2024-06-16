using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects
{
    public class SignInteractor : PassableObjectInteractor
    {
        public override void OnEntering(InteractingWithObject state)
        {
            string title = "A sign reads:";
            string text = (state.Obj as Sign).GetTitle();

            DialogUI.Instance.ShowDialogUI(title, text, null);

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
            DialogUI.Instance.HideDialog();

            GameStateManager.Instance().SetState(
                new Adventuring()
                );

            MoveToObject(state.Obj.X, state.Obj.Y);
        }

        public override void OnVisiting(InteractingWithObject state)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance().SetState(
                    new InteractingWithObject
                    {
                        Stage = InteractingWithObject.InteractingStage.exiting,
                        Obj = state.Obj,
                    }
                    );
            }
        }
    }
}