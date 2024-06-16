using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Accounting.Transactions;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Combat;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Data.State.Adventure;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Adventure.UI.Background;
using Assets.Scripts.Adventure.UI.BottomPanel;
using Assets.Resources.ScriptableObjects;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects
{
    public class DwellingInteractor : BaseObjectInteractor
    {
        public enum DwellingInnerState
        {
            none,
            waitForConfirmation
        }

        private static DwellingInnerState innerState;

        private static Dwelling currentDwelling;

        private static readonly Dictionary<DwellingInnerState, System.Action<InteractingWithObject>> inputProcessors =
                new()
                {
            { DwellingInnerState.none, ProcessDefaultInput },
            { DwellingInnerState.waitForConfirmation, ProcessConfirmationInput },
                };

        public override void OnEntering(InteractingWithObject state)
        {
            var dwelling = state.Obj as Dwelling;

            currentDwelling = dwelling;

            BottomUIScript.Instance.ShowBottomUI();
            BackgroundUIScript.Instance.ShowObjectBackground(state.Obj);

            innerState = DwellingInnerState.none;

            GameStateManager.Instance().SetState(
                new InteractingWithObject
                {
                    Stage = InteractingWithObject.InteractingStage.visiting,
                    Obj = state.Obj,
                    exitingState = state.exitingState,
                }
            );

            EventBus.Instance.PostEvent(new OnEnterDwelling
            {
                Dwelling = dwelling,
                PurchaseCallback = OnPurchaseCallback
            });
        }

        public override void OnExiting(InteractingWithObject state)
        {
            currentDwelling = null;

            if (state.exitingState is Adventuring)
            {
                BottomUIScript.Instance.HideBottomUI();
                BackgroundUIScript.Instance.HideBackground();
            }

            GameStateManager.Instance().SetState(
                state.exitingState
                );

            EventBus.Instance.PostEvent(new OnLeaveDwelling());
        }

        public override void OnVisiting(InteractingWithObject state)
        {
            inputProcessors[innerState](state);
        }

        private static void ProcessDefaultInput(InteractingWithObject state)
        {
            var dwelling = state.Obj as Dwelling;
            string title = dwelling.GetDwellingType().ToString();
            string text = dwelling.CurrentPopulation()
                + " "
                + dwelling.Unit.Name + " are available \r\n";

            text += "Cost = " + dwelling.Unit.Cost + " each \r\n";
            text += "You may recruit up to " + ComputeRecruitingNumber(
                PlayerSquads.Instance().GetSquads(),
                dwelling.Unit,
                PlayerStats.Instance().GetLeadership()
                );

            string currentGold = "Gold = " + PlayerStats.Instance().GetGold();

            bool canRecruit = PlayerSquads.Instance().CanHireSquad(dwelling.Unit);
            string recruitHowMany;
            if (canRecruit)
            {
                recruitHowMany = "Recruit how many ?";
            }
            else
            {
                recruitHowMany = "Your army is full";
            }

            BottomUIScript.Instance.ShowUIMessage(title, text, currentGold, recruitHowMany, canRecruit);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance().SetState(
                    new InteractingWithObject
                    {
                        Stage = InteractingWithObject.InteractingStage.exiting,
                        Obj = state.Obj,
                        exitingState = state.exitingState,
                    }
                    );
            }
        }

        private static void ProcessConfirmationInput(InteractingWithObject state)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                innerState = DwellingInnerState.none;
            }
        }

        public void OnPurchaseCallback(int amount)
        {
            var squads = PlayerSquads.Instance().GetSquads();
            int leadership = PlayerStats.Instance().GetLeadership();

            var transaction = new BuySquadTransaction(
                    currentDwelling.Unit,
                    amount,
                    ComputeRecruitingNumber(squads, currentDwelling.Unit, leadership),
                    currentDwelling.CurrentPopulation()
                    );

            string text = string.Empty;

            switch ((BuySquadTransaction.TransactionResult)transaction.Result())
            {
                case BuySquadTransaction.TransactionResult.hasBeenPurchased:
                    currentDwelling.ChangePopulation(-amount);
                    innerState = DwellingInnerState.none;

                    break;

                case BuySquadTransaction.TransactionResult.notEnoughGold:
                    innerState = DwellingInnerState.waitForConfirmation;
                    text = "You dont have enough gold...";

                    break;

                case BuySquadTransaction.TransactionResult.notEnoughPopulation:
                    innerState = DwellingInnerState.waitForConfirmation;
                    text = "The population is too small...";

                    break;

                case BuySquadTransaction.TransactionResult.notEnoughLeadership:
                    innerState = DwellingInnerState.waitForConfirmation;
                    text = "You dont have enough leadership yet...";

                    break;

                case BuySquadTransaction.TransactionResult.cantBuyZeroAmount:
                    innerState = DwellingInnerState.none;

                    break;
            }

            BottomUIScript.Instance.UpdateTextMessage(text);
        }

        private static int ComputeRecruitingNumber(List<UnitGroup> squads, UnitScriptableObject unit, int leadership)
        {
            int potentialQuantity = leadership / unit.HP;
            int initialQuantity = squads.Aggregate(0, (acc, squad) =>
            {
                int count = squad.Unit.Name == unit.Name ? squad.CurrentQuantity() : 0;

                return acc + count;
            });

            return Mathf.Max(0, potentialQuantity - initialQuantity);
        }
    }
}