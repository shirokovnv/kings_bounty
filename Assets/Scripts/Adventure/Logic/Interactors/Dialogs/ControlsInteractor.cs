using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Dialogs
{
    public class ControlsInteractor : BaseDialogInteractor<Adventuring, ViewControls>
    {
        private readonly static string[] controls = new string[]
        {
        "\u21d0 : go west",
        "\u21d1 : go north",
        "\u21d2 : go east",
        "\u21d3 : go south",
        "Home: go north-west",
        "PgUp: go north-east",
        "End: go south-west",
        "PgDown: go south-east",
        "A: view army",
        "C: controls",
        "D: dismiss",
        "H: help",
        "I: view contract",
        "L: load",
        "M: show map",
        "N: new continent",
        "P: puzzle solve",
        "Q: search",
        "S: save",
        "U: cast spell",
        "V: view character",
        "W: wait a week",
        "Esc: exit"
        };

        public override void WaitForStart()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                string title = "Controls:";

                string text = string.Join("\r\n", controls);

                DialogUI.Instance.ShowDialogUI(title, text, null);

                GameStateManager.Instance().SetState(new ViewControls());
            }
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