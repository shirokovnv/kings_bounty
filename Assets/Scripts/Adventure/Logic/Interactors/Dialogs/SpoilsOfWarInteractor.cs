using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.Logic.Puzzle;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Combat.Logic.Systems;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Data.State.Combat;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Logic.Character.Ranks;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Dialogs
{
    public class SpoilsOfWarInteractor : BaseDialogInteractor<WinCombat, ViewSpoilsOfWar>
    {
        public override void WaitForStart()
        {
            WinCombat state = GameStateManager.Instance().GetState() as WinCombat;

            var combatable = state.GetCombatable();
            var obj = combatable.GetObject();

            var continent = ContinentSystem.Instance().GetContinentAtNumber(obj.ContinentNumber);

            string title = "Victory!";
            string text = string.Empty;

            text += $"Well done {Player.Instance().GetFullTitle()} \r\n";
            text += "you have successfully vanquished \r\n";
            text += "yet another foe! \r\n";

            text += "\r\n";

            text += "Spoils of war: " + combatable.GetSpoilsOfWar() + " gold. \r\n";

            if (Player.Instance().GetRank().GetBaseRank() == BaseRank.Knight)
            {
                text += "\r\n";
                text += $"As a noble knight, you get leadership bonus: {combatable.GetLeadership()}";
                text += "\r\n";

                PlayerStats.Instance().ChangeLeadership(combatable.GetLeadership());
            }

            if (obj.GetObjectType() == ObjectType.captain)
            {
                continent.GetGrid().GetValue(obj.X, obj.Y).ObjectLayer = null;
            }

            if (obj.GetObjectType() == ObjectType.castleGate)
            {
                var castle = obj as Castle;

                if (castle.IsContracted())
                {
                    text += "\r\n";

                    var contract = ContractSystem.Instance().GetContractByCastlePosition(
                        castle.X, castle.Y, castle.ContinentNumber
                        );

                    var currentContract = ContractSystem.Instance().GetCurrentContract();

                    if (contract != null)
                    {
                        if (contract == currentContract)
                        {
                            text += $"For fullfilling your contract by capturing of {contract.ContractInfo().Title}, \r\n";
                            text += $"you receive an additional {contract.Reward()} gold... \r\n";
                            text += "and a piece of the map to the Lost Grail.";

                            PlayerStats.Instance().IncreaseVillainsCaught();
                            PlayerStats.Instance().ChangeGold(contract.Reward());
                            PuzzleSystem.Instance().RevealPuzzlePiece(
                                contract.ContractInfo().GetScriptID(),
                                PuzzlePiece.PuzzleType.Contract
                                );

                            EventBus.Instance.PostEvent(new OnNewContract { Contract = null });
                            ContractSystem.Instance().ClearContract();
                        }
                        else
                        {
                            text += "Since you did not have the proper contract, the Lord has been set free.\r\n";
                            text += "And a piece of the map to the Lost Grail disappears...";
                        }
                    }

                    contract.SetFinished(true);
                    castle.SetIsContracted(false);
                }

                castle.RemoveAllSquads();
                castle.SetOwner(CastleOwner.player);

                PlayerStats.Instance().IncreaseCastlesGarrisoned();
            }

            int remainsQuantity = PlayerSquads.Instance().GetSquadsTotalQuantity();
            int diffQuantity = Mathf.Max(0, state.GetTotalQuantity() - remainsQuantity);

            PlayerStats.Instance().IncreaseFollowersKilled(diffQuantity);
            PlayerStats.Instance().ChangeGold(combatable.GetSpoilsOfWar());

            MoraleSystem.Instance().CalculateMorale(PlayerSquads.Instance().GetSquads());

            EventBus.Instance.PostEvent(new OnWinCombat { Combatable = combatable });
            EventBus.Instance.PostEvent(new OnChangePosition(Player.Instance().X, Player.Instance().Y));

            GameStateManager.Instance().SetState(new ViewSpoilsOfWar(
                state.GetCombatable(),
                state.GetTotalQuantity()
                ));

            DialogUI.Instance.ShowDialogUI(title, text, null);
        }

        public override void WaitForFinish()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance().SetState(new Adventuring());

                DialogUI.Instance.HideDialog();
            }
        }
    }
}