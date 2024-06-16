using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Accounting.Transactions;
using Assets.Scripts.Adventure.Logic.Accounting.Transactions.Factory;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Adventure.UI.Background;
using Assets.Scripts.Adventure.UI.BottomPanel;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects
{
    public class TownInteractor : BaseObjectInteractor
    {
        private enum TownInnerState
        {
            none,
            waitForConfirmation,
        }

        private static TownInnerState innerState;

        private static readonly Dictionary<TownInnerState, System.Action<InteractingWithObject>> inputProcessors =
            new()
            {
            { TownInnerState.none, ProcessDefaultInput },
            { TownInnerState.waitForConfirmation, ProcessConfirmationInput },
            };

        public override void OnEntering(InteractingWithObject state)
        {
            BottomUIScript.Instance.ShowBottomUI();
            BackgroundUIScript.Instance.ShowObjectBackground(state.Obj);

            innerState = TownInnerState.none;

            GameStateManager.Instance().SetState(
                new InteractingWithObject
                {
                    Stage = InteractingWithObject.InteractingStage.visiting,
                    Obj = state.Obj,
                }
            );

            (state.Obj as City).SetVisited(true);

            EventBus.Instance.PostEvent(new OnEnterTown());
        }

        public override void OnExiting(InteractingWithObject state)
        {
            BottomUIScript.Instance.HideBottomUI();
            BackgroundUIScript.Instance.HideBackground();

            GameStateManager.Instance().SetState(
                new Adventuring()
                );

            EventBus.Instance.PostEvent(new OnLeaveTown());
        }

        public override void OnVisiting(InteractingWithObject state)
        {
            inputProcessors[innerState](state);
        }

        private static void ProcessDefaultInput(InteractingWithObject state)
        {
            var city = state.Obj as City;

            string title = $"Town of {state.Obj.GetName()}";
            string text = string.Empty;

            text += "A) Get a contract \r\n";

            if (Boat.Instance().IsActive)
            {
                text += "B) Cancel boat rental \r\n";
            }
            else
            {
                if (Player.Instance().IsNavigating())
                {
                    text += "B) Cancel boat rental \r\n";
                }
                else
                {
                    text += "B) Rent boat \r\n";
                }
            }

            text += "C) Gather castle info \r\n";
            text += $"D) Buy spell {city.Spell.Name} ({city.Spell.Cost}) \r\n";
            text += "E) Buy siege weapon";

            string gold = "Gold = " + PlayerStats.Instance().GetGold();

            BottomUIScript.Instance.ShowUIMessage(title, text, gold);

            if (Input.GetKeyDown(KeyCode.A))
            {
                var contract = ContractSystem.Instance().GetNewContract();

                var currentContract = ContractSystem.Instance().GetCurrentContract();

                var (cTitle, cBody, sprite) = (default(string), default(string), null as Sprite);

                if (currentContract != null)
                {
                    cTitle = $"{currentContract.ContractInfo().Title}";

                    cBody += $"Alias: {currentContract.ContractInfo().Alias}\r\n";
                    cBody += $"Reward: {currentContract.Reward()} \r\n";
                    cBody += $"Last seen: {currentContract.LastSeen()} \r\n";
                    cBody += $"Castle: {currentContract.Castle()} \r\n";

                    cBody += $"Distinguishing features: \r\n";
                    cBody += Utils.Split(currentContract.ContractInfo().DistinguishingFeatures, 50, " ", "\r\n\t")
                        + "\r\n";

                    cBody += $"Crimes: \r\n";
                    cBody += Utils.Split(currentContract.ContractInfo().Crimes, 50, " ", "\r\n\t")
                        + "\r\n";

                    sprite = currentContract.ContractInfo().Sprites[0];
                }

                DialogUI.Instance.ShowDialogUI(cTitle, cBody, sprite);

                innerState = TownInnerState.waitForConfirmation;

                EventBus.Instance.PostEvent(new OnNewContract { Contract = contract });
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                var transaction = BoatTransactionFactory.Create();

                switch (transaction)
                {
                    case BuyBoatTransaction:
                        text = BuyBoatMessage((BuyBoatTransaction)transaction, city);
                        break;
                    case CancelBoatTransaction:
                        text = CancelBoatMessage((CancelBoatTransaction)transaction);
                        break;
                }

                BottomUIScript.Instance.ShowUIMessage(title, text);
                innerState = TownInnerState.waitForConfirmation;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                var town = state.Obj as City;
                var castle = town.GetLinkedCastle();

                text = $"Castle {castle.GetName()} is under the rule of ";

                if (castle.GetOwner() == CastleOwner.opponent)
                {
                    if (castle.IsContracted())
                    {
                        var contract = ContractSystem.Instance().GetContractByCastlePosition(
                            castle.X, castle.Y, castle.ContinentNumber
                            );

                        contract.SetLastSeen(
                            ContinentSystem.Instance().GetContinentAtNumber(castle.ContinentNumber).GetName()
                            );
                        contract.SetCastle(castle.GetName());

                        text += $"{contract.ContractInfo().Title}";
                    }
                    else
                    {
                        text += "no one";
                    }
                }

                if (castle.GetOwner() == CastleOwner.player)
                {
                    text += $"{Player.Instance().GetFullTitle()}";
                }

                if (castle.GetOwner() == CastleOwner.king)
                {
                    text += "King Maximus";
                }

                text += ": \r\n \r\n";

                town.GetLinkedCastle().GetSquads().ForEach(group =>
                    {
                        text += $"{group.Unit.Name} : {group.CurrentQuantity()} \r\n";
                    });

                BottomUIScript.Instance.ShowUIMessage(title, text);
                innerState = TownInnerState.waitForConfirmation;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                var transaction = new BuySpellTransaction(city.Spell);
                text = BuySpellMessage(transaction);

                BottomUIScript.Instance.ShowUIMessage(title, text);
                innerState = TownInnerState.waitForConfirmation;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                var transaction = new BuySiegeWeaponTransaction();

                switch ((BuySiegeWeaponTransaction.TransactionResult)transaction.Result())
                {
                    case BuySiegeWeaponTransaction.TransactionResult.hasBeenPurchased:
                        text = "Siege weapon has been purchased.";

                        EventBus.Instance.PostEvent(new OnBuySiegeWeaponEvent
                        {
                            IsPurchased = true
                        });

                        break;

                    case BuySiegeWeaponTransaction.TransactionResult.notEnoughGold:
                        text = "You dont have enough gold to buy siege weapon.";

                        break;

                    case BuySiegeWeaponTransaction.TransactionResult.alreadyPurchased:
                        text = "You already have siege weapon.";

                        break;
                }

                BottomUIScript.Instance.ShowUIMessage(title, text);
                innerState = TownInnerState.waitForConfirmation;
            }

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

        private static void ProcessConfirmationInput(InteractingWithObject state)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DialogUI.Instance.HideDialog();

                innerState = TownInnerState.none;
            }
        }

        private static string BuyBoatMessage(BuyBoatTransaction transaction, City city)
        {
            if (!city.CanLaunchBoat())
            {
                return "Can't launch boat...";
            }

            string text = string.Empty;

            switch ((BuyBoatTransaction.TransactionResult)transaction.Result())
            {
                case BuyBoatTransaction.TransactionResult.hasBeenPurchased:
                    text = "Boat has been purchased...";
                    Vector2Int boatPosition = city.LaunchTheBoat(Player.Instance().X, Player.Instance().Y);
                    EventBus.Instance.PostEvent(new OnBuyBoat(boatPosition.x, boatPosition.y, city.ContinentNumber));

                    break;

                case BuyBoatTransaction.TransactionResult.notEnoughGold:
                    text = "You dont have enough gold for purchasing boat...";

                    break;

                case BuyBoatTransaction.TransactionResult.alreadyHaveBoat:
                    text = "You already have boat...";

                    break;
            }

            return text;
        }

        private static string CancelBoatMessage(CancelBoatTransaction transaction)
        {
            string text = string.Empty;

            switch ((CancelBoatTransaction.TransactionResult)transaction.Result())
            {
                case CancelBoatTransaction.TransactionResult.hasBeenCanceled:
                    text = "Boat has been canceled...";
                    EventBus.Instance.PostEvent(new OnCancelBoat());

                    break;

                case CancelBoatTransaction.TransactionResult.dontHaveBoat:
                    text = "You dont have boat...";

                    break;

                case CancelBoatTransaction.TransactionResult.needToGoAshore:
                    text = "You need to go ashore first...";

                    break;
            }

            return text;
        }

        private static string BuySpellMessage(BuySpellTransaction transaction)
        {
            string text = string.Empty;

            switch ((BuySpellTransaction.TransactionResult)transaction.Result())
            {
                case BuySpellTransaction.TransactionResult.hasBeenPurchased:
                    int learnCount = PlayerStats.Instance().GetKnowledge() - Spellbook.Instance().TotalNumberOfSpells();
                    text = "You can learn " + learnCount + " more spells";
                    break;

                case BuySpellTransaction.TransactionResult.notEnoughGold:
                    text = "You dont have enough gold to buy a spell...";
                    break;

                case BuySpellTransaction.TransactionResult.notEnoughKnowledge:
                    text = "You dont have enough knowledge to learn a spell";
                    break;
            }

            return text;
        }
    }
}