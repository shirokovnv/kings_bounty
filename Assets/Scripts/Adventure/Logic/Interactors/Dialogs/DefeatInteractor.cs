using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Data.State.Combat;
using UnityEngine;
using Assets.Scripts.Adventure.UI.Dialog;

namespace Assets.Scripts.Adventure.Logic.Interactors.Dialogs
{
    public class DefeatInteractor : BaseDialogInteractor<LoseCombat, ViewDefeatMessage>
    {
        public override void WaitForStart()
        {
            LoseCombat state = GameStateManager.Instance().GetState() as LoseCombat;

            string title = "Defeat!";
            string text = string.Empty;
            text += "After beign disgraced on the field of battle, \r\n";
            text += "King Maximus summons you to his castle. \r\n";
            text += "After a lesson in tactics, he reluctantly \r\n";
            text += "reissues your commission and sends you \r\n";
            text += "on your way.";

            var position = ContinentSystem.Instance().GetContinentAtNumber(1).GetStartPosition();

            PlayerSquads.Instance().SetBaseSquads(Player.Instance().GetRank());

            EventBus.Instance.PostEvent(new OnLoseCombat());
            EventBus.Instance.PostEvent(new OnChangeContinent(1));
            EventBus.Instance.PostEvent(new OnChangePosition(position.x, position.y));

            Player.Instance().SetNavigating(false);
            Boat.Instance().IsActive = false;

            Boat.Instance().X = -1;
            Boat.Instance().Y = -1;

            PlayerStats.Instance().IncreaseFollowersKilled(state.GetTotalQuantity());

            // refresh squads
            PlayerSquads.Instance().SetBaseSquads(Player.Instance().GetRank());

            GameStateManager.Instance().SetState(new ViewDefeatMessage(
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