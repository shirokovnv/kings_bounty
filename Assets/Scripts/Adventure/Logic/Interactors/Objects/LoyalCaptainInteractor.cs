using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Accounting.Transactions;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Data.State.Adventure;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Adventure.UI.BottomPanel;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects
{
    public class LoyalCaptainInteractor : BaseCombatableInteractor
    {
        private static bool isJoinSuccessful;

        public override void OnEntering(InteractingWithObject state)
        {
            isJoinSuccessful = true;

            var captain = state.Obj as Captain;

            string title = "Your scouts have sighted: \r\n";
            string text = "\r\n";

            var squad = captain.GetSquads().ElementAt(0);

            text += $"{ApproximateQuantity(squad.CurrentQuantity())} {squad.Unit.Name} with desires of ";
            text += "greater glory, wish to join you.";

            string actionText = "Accept? (Y/N)";

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
            var captain = state.Obj as Captain;

            ContinentSystem
                .Instance()
                .GetContinentAtNumber(captain.ContinentNumber)
                .GetGrid()
                .GetValue(captain.X, captain.Y)
                .ObjectLayer = null;

            EventBus.Instance.PostEvent(new OnJoinSquad
            {
                X = captain.X,
                Y = captain.Y,
                ContinentNumber = captain.ContinentNumber,
            });

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

            if (!isJoinSuccessful)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                var captain = state.Obj as Captain;

                var squad = captain.GetSquads().ElementAt(0);

                var transaction = new JoinSquadTransaction(squad);
                isJoinSuccessful = transaction.Result() == (int)JoinSquadTransaction.TransactionResult.Success;

                if (isJoinSuccessful)
                {
                    GameStateManager.Instance().SetState(
                        new InteractingWithObject
                        {
                            Stage = InteractingWithObject.InteractingStage.exiting,
                            Obj = state.Obj,
                        }
                        );
                }
                else
                {
                    BottomUIScript.Instance.UpdateTextMessage($"{squad.Unit.Name} flee in terror at the sight your vast army...");
                }
            }
        }
    }
}