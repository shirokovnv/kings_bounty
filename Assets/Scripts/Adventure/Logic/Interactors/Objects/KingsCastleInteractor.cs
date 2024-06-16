using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.UI.Background;
using Assets.Scripts.Adventure.UI.BottomPanel;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Logic.Character;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects
{
    public class KingsCastleInteractor : BaseObjectInteractor
    {
        private const string TITLE = "Castle of King Maximus";

        private readonly KeyCode[] keyCodes = new KeyCode[]
        {
            KeyCode.A,
            KeyCode.B,
            KeyCode.C,
            KeyCode.D,
            KeyCode.E,
        };

        private readonly int[] requirePromotions = new[] { 0, 0, 0, 1, 2 };

        private enum KingCastleState
        {
            None,
            BuySquads,
            Audience,
            Promotion
        }

        private static KingCastleState castleState;

        public override void OnEntering(InteractingWithObject state)
        {
            castleState = KingCastleState.None;

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
            BackgroundUIScript.Instance.HideBackground();

            GameStateManager.Instance().SetState(
                new Adventuring()
                );
        }

        public override void OnVisiting(InteractingWithObject state)
        {
            switch (castleState)
            {
                case KingCastleState.None:
                    ProcessMainMenuInput(state);
                    break;

                case KingCastleState.BuySquads:
                    ProcessBuySquadsInput(state);
                    break;

                case KingCastleState.Audience:
                    ProcessAudienceInput();
                    break;

                case KingCastleState.Promotion:
                    ProcessPromotionInput();
                    break;
            }
        }

        private void ProcessMainMenuInput(InteractingWithObject state)
        {
            var castle = state.Obj as Castle;

            string text = string.Empty;
            text += "A) Buy squads \r\n";
            text += "B) Audience with the King";

            BottomUIScript.Instance.ShowBottomUI();
            BottomUIScript.Instance.ShowUIMessage(TITLE, text);
            BackgroundUIScript.Instance.ShowObjectBackground(castle);

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

            if (Input.GetKeyDown(KeyCode.A))
            {
                castleState = KingCastleState.BuySquads;
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                castleState = KingCastleState.Audience;
            }
        }

        private void ProcessBuySquadsInput(InteractingWithObject state)
        {
            string text = string.Empty;

            var castleUnits = UnitManager.Instance().GetUnitsByType(Dwelling.DwellingType.castle);

            castleUnits = castleUnits.OrderBy(x => x.Level).ThenBy(x => x.HP).ToList();

            var promotedTimes = Player.Instance().GetRank().GetPromotions().PromotedTimes();

            for (int i = 0; i < castleUnits.Count; i++)
            {
                text += char.ToUpper(Convert.ToChar(keyCodes[i])) + ") " + castleUnits[i].Name + " ";

                if (promotedTimes < requirePromotions[i])
                {
                    text += "(N/A)";
                }

                text += "\r\n";
            }

            BottomUIScript.Instance.ShowUIMessage(TITLE, text);

            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]) && requirePromotions[i] <= promotedTimes)
                {
                    var dwelling = Dwelling.CreateVirtual(castleUnits[i]);
                    dwelling.SetName(TITLE);
                    var nextState = new InteractingWithObject
                    {
                        Obj = dwelling,
                        Stage = InteractingWithObject.InteractingStage.entering,
                        exitingState = state
                    };

                    GameStateManager.Instance().SetState(nextState);

                    return;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                castleState = KingCastleState.None;
            }
        }

        public void ProcessAudienceInput()
        {
            var nextPromotion = Player.Instance().GetRank().GetPromotions().NextPromotion();

            string text = string.Empty;
            text += "King Maximus rises from his throne and proclaims: \r\n";

            if (nextPromotion != null)
            {
                if (nextPromotion.CountOfVillainsToBeCaught() > PlayerStats.Instance().GetVillainsCaught())
                {
                    int villainsCaughtRemains =
                        nextPromotion.CountOfVillainsToBeCaught() - PlayerStats.Instance().GetVillainsCaught();

                    text += "- My Dear, " + Player.Instance().GetName() + "! \r\n";
                    text += "I can aid you better after you have captured "
                        + villainsCaughtRemains + " more villains.";
                }
                else
                {
                    text += "- Congratulations! \r\n";
                    text += "I now promote you to " + nextPromotion.GetPromotionName() + "\r\n";

                    Player.Instance().GetRank().GetPromotions().Promote();

                    PlayerStats.Instance().ChangeLeadership(nextPromotion.LeadershipBonus());
                    PlayerStats.Instance().ChangeSpellPower(nextPromotion.SpellPowerBonus());
                    PlayerStats.Instance().ChangeKnowledge(nextPromotion.KnowledgeBonus());
                    PlayerStats.Instance().ChangeWeekSalary(nextPromotion.WeekCommissionBonus());

                    castleState = KingCastleState.Promotion;
                }
            }
            else
            {
                text += "- Hurry and recover my Scepter of Order or all will be lost!";
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                castleState = KingCastleState.None;
            }

            BottomUIScript.Instance.UpdateTextMessage(text);
        }

        public void ProcessPromotionInput()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            {
                castleState = KingCastleState.None;
            }
        }
    }
}