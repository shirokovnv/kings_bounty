using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.Logic.Interactors.Objects.Picker;
using Assets.Scripts.Adventure.Logic.Puzzle;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects
{
    public class ArtifactInteractor : PassableObjectInteractor
    {
        public override void OnEntering(InteractingWithObject state)
        {
            var artifact = state.Obj as Artifact;

            string title = artifact.ArtifactScript.Name;
            string text = artifact.ArtifactScript.Description;
            Sprite sprite = artifact.ArtifactScript.InventorySprite;

            DialogUI.Instance.ShowDialogUI(title, text, sprite);

            GameStateManager.Instance().SetState(
                new InteractingWithObject
                {
                    Obj = state.Obj,
                    Stage = InteractingWithObject.InteractingStage.visiting
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
                ArtifactPicker.PickUp((state.Obj as Artifact).ArtifactScript);
                Inventory.Instance().AddArtifact(state.Obj as Artifact);
                PuzzleSystem.Instance().RevealPuzzlePiece(
                    (state.Obj as Artifact).ArtifactScript.GetScriptID(),
                    PuzzlePiece.PuzzleType.Artifact
                    );

                PlayerStats.Instance().IncreaseArtifactsFound();

                GameStateManager.Instance().SetState(
                    new InteractingWithObject
                    {
                        Obj = state.Obj,
                        Stage = InteractingWithObject.InteractingStage.exiting
                    }
                );

                var artifact = state.Obj as Artifact;
                var continent = ContinentSystem.Instance().GetContinentAtNumber(artifact.ContinentNumber);

                continent.GetGrid().GetValue(artifact.X, artifact.Y).ObjectLayer = null;
                EventBus.Instance.PostEvent(new OnPickObject
                {
                    X = artifact.X,
                    Y = artifact.Y,
                    ContinentNumber = artifact.ContinentNumber
                });
            }
        }
    }
}