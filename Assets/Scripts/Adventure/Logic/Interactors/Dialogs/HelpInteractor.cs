using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Dialogs
{
    public class HelpInteractor : BaseDialogInteractor<Adventuring, ViewHelp>
    {
        private static string TITLE = "----- HELP -----";

        private static string[] TEXT =
        {
            "The game is a remake of original 1990 Kings bounty.",
            "The main goal is to find Legendary Artifact in time.",
            "If the time is out - you lose.",
            "The knight in the center of the screen on a horse - is your hero.",
            "The lands you are exploring are divided into several continents.",
            "You are on the first continent.",
            "To go futher you need to find a map from one of the chests.",
            "Next to you to the north is the castle of King Maximus.",
            "And there is a city nearby where you can buy a ship for sea trips and not only.",
            "The game features turn-based battles.",
            "To see available options press (O) button.",
            "Most of the time for cancel/skip dialogs press (Esc) button.",
            "Good luck!"
        };

        public override void WaitForStart()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                DialogUI.Instance.ShowDialogUI(TITLE, string.Join("\r\n\r\n", TEXT), null);

                GameStateManager.Instance().SetState(new ViewHelp());
            }
        }

        public override void WaitForFinish()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DialogUI.Instance.HideDialog();

                GameStateManager.Instance().SetState(new Adventuring());
            }
        }
    }
}
