using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Accounting.Transactions;
using Assets.Scripts.Adventure.Logic.Continents.Interactors.Systems;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Data.State.Adventure;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Adventure.UI.Dialog;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Objects
{
    public class ChestInteractor : PassableObjectInteractor
    {
        private const int CHEST_LEADERSHIP_PENALTY = 50;
        private const int CHEST_SALARY_PENALTY = 20;

        private static readonly Dictionary<Chest.ChestType, System.Action<InteractingWithObject>> inputProcessors =
            new() {
            { Chest.ChestType.Treasure, ProcessTreasureChestInput },
            { Chest.ChestType.Commission, ProcessSalaryChestInput },
            { Chest.ChestType.Spell, ProcessSpellChestInput },
            { Chest.ChestType.Spellpower, ProcessSpellPowerChestInput },
            { Chest.ChestType.Knowledge, ProcessKnowledgeChestInput },
            { Chest.ChestType.MapReveal, ProcessMapRevealChestInput },
            { Chest.ChestType.PathToContinent, ProcessPathToContinentChestInput }
            };

        public override void OnEntering(InteractingWithObject state)
        {
            var chest = state.Obj as Chest;

            string title = string.Empty;
            string text = string.Empty;

            var type = chest.GetChestType();
            var value = chest.GetChestValue();

            switch (type)
            {
                case Chest.ChestType.Treasure:
                    title = "Treasure";

                    text += "After scouting the area, \r\n";
                    text += "you fall upon a hidden treasure cache. \r\n";
                    text += "You may: \r\n";
                    text += "\r\n";
                    text += $"A) Take {value} gold \r\n";
                    text += $"B) Distribute ({value / CHEST_LEADERSHIP_PENALTY} leadership) \r\n";
                    break;
                case Chest.ChestType.Commission:
                    title = "Event!";

                    text += "After scouting the area, \r\n";
                    text += "you discover that it is \r\n";
                    text += "rich in mineral deposits. \r\n";
                    text += "The King rewards you for \r\n";
                    text += "your find by increasing \r\n";
                    text += $"your weekly income by {value / CHEST_SALARY_PENALTY}.";

                    break;
                case Chest.ChestType.Spell:
                    var spell = SpellManager.Instance().GetSpellAt(chest.GetChestValue());

                    title = "Event!";

                    text += "You have captured a mischievous \r\n";
                    text += "imp which has been terrorizing \r\n";
                    text += "the region. In exchange for \r\n";
                    text += "its release, you receive: \r\n";
                    text += $"1 spell ({spell.Name}).";

                    break;
                case Chest.ChestType.Spellpower:
                    title = "Event!";

                    text += "Traversing the area, you \r\n";
                    text += "stumble upon a time worn \r\n";
                    text += "cannister. Curious, you unstop \r\n";
                    text += "the bottle, releasing \r\n";
                    text += "a powerful genie who raises";
                    text += $"your Spell Power by {value} and vanishes.";

                    break;

                case Chest.ChestType.Knowledge:
                    title = "Event!";

                    text += "A tribe of nomads greet you \r\n";
                    text += "and your army warmly. Their \r\n";
                    text += "shaman, in awe of your prowes, \r\n";
                    text += "teaches you the secret of his \r\n";
                    text += "tribes magic. \r\n";
                    text += $"Your knowledge is increased by {value}";

                    break;
                case Chest.ChestType.MapReveal:
                    title = "Event!";

                    text += "Peering through a magical orb \r\n";
                    text += "you are able to view the entire \r\n";
                    text += "continent. Your map of this \r\n";
                    text += "area is complete.";

                    break;
                case Chest.ChestType.PathToContinent:
                    title = "Event!";

                    var nextContinent = ContinentSystem
                        .Instance()
                        .GetContinentAtNumber(chest.ContinentNumber + 1);

                    text += "Hidden within an ancient chest, you \r\n";
                    text += "find maps and charts describing \r\n";
                    text += $"passage to {nextContinent.GetName()}.";

                    break;
            }

            DialogUI.Instance.ShowDialogUI(title, text, null);

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
            var chest = state.Obj as Chest;

            inputProcessors[chest.GetChestType()](state);
        }

        private static void ProcessTreasureChestInput(InteractingWithObject state)
        {
            var chest = state.Obj as Chest;
            bool wasProcessed = false;

            if (Input.GetKeyDown(KeyCode.A))
            {
                // get the gold
                PlayerStats.Instance().ChangeGold(chest.GetChestValue());
                wasProcessed = true;
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                // distribute
                PlayerStats.Instance().ChangeLeadership(chest.GetChestValue() / CHEST_LEADERSHIP_PENALTY);
                wasProcessed = true;
            }

            if (wasProcessed)
            {
                PickUpTheChest(chest);
            }
        }

        private static void ProcessSalaryChestInput(InteractingWithObject state)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var chest = state.Obj as Chest;
                var amount = chest.GetChestValue() / CHEST_SALARY_PENALTY;
                var transaction = new ChangeSalaryTransaction(amount);
                transaction.Result();

                PickUpTheChest(chest);
            }
        }

        private static void ProcessSpellChestInput(InteractingWithObject state)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var chest = state.Obj as Chest;
                var spell = SpellManager.Instance().GetSpellAt(chest.GetChestValue());
                Spellbook.Instance().AddSpell(spell);

                PickUpTheChest(chest);
            }
        }

        private static void ProcessSpellPowerChestInput(InteractingWithObject state)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var chest = state.Obj as Chest;
                PlayerStats.Instance().ChangeSpellPower(chest.GetChestValue());

                PickUpTheChest(chest);
            }
        }

        private static void ProcessKnowledgeChestInput(InteractingWithObject state)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var chest = state.Obj as Chest;
                PlayerStats.Instance().ChangeKnowledge(chest.GetChestValue());

                PickUpTheChest(chest);
            }
        }

        private static void ProcessMapRevealChestInput(InteractingWithObject state)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var chest = state.Obj as Chest;

                ContinentSystem
                    .Instance()
                    .GetContinentAtNumber(chest.ContinentNumber)
                    .SetHasFullMap(true);

                PickUpTheChest(chest);
            }
        }

        private static void ProcessPathToContinentChestInput(InteractingWithObject state)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var chest = state.Obj as Chest;

                ContinentSystem
                    .Instance()
                    .GetContinentAtNumber(chest.ContinentNumber + 1)
                    .SetRevealed(true);

                PickUpTheChest(chest);
            }
        }

        private static void PickUpTheChest(Chest chest)
        {
            var continent = ContinentSystem.Instance().GetContinentAtNumber(chest.ContinentNumber);

            continent.GetGrid().GetValue(chest.X, chest.Y).ObjectLayer = null;
            EventBus.Instance.PostEvent(new OnPickObject
            {
                X = chest.X,
                Y = chest.Y,
                ContinentNumber = chest.ContinentNumber,
            });

            GameStateManager.Instance().SetState(
                new InteractingWithObject
                {
                    Obj = chest,
                    Stage = InteractingWithObject.InteractingStage.exiting
                }
            );
        }
    }
}