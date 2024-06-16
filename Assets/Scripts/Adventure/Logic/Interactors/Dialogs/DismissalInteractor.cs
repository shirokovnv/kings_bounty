using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Data.State.Adventure;
using System;
using UnityEngine;
using Assets.Scripts.Adventure.UI.Dialog;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Dialogs
{
    public class DismissalInteractor : BaseDialogInteractor<Adventuring, ViewDismissal>
    {
        private readonly KeyCode[] keyCodes = new KeyCode[]
        {
        KeyCode.A,
        KeyCode.B,
        KeyCode.C,
        KeyCode.D,
        KeyCode.E,
        };

        public override void WaitForStart()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                GameStateManager.Instance().SetState(new ViewDismissal());

                var squads = PlayerSquads.Instance().GetSquads();

                string title = "Choose army to dismiss";
                string text = string.Empty;

                for (int i = 0; i < squads.Count; i++)
                {
                    var squad = squads[i];

                    text += char.ToUpper(Convert.ToChar(keyCodes[i])) + ") "
                        + squad.Unit.Name + " : "
                        + squad.CurrentQuantity() + "\r\n";
                }

                DialogUI.Instance.ShowDialogUI(title, text, null);
            }
        }

        public override void WaitForFinish()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance().SetState(new Adventuring());

                DialogUI.Instance.HideDialog();
            }

            var squads = PlayerSquads.Instance().GetSquads();

            if (squads.Count <= 1)
            {
                return;
            }

            for (int i = 0; i < squads.Count; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    PlayerSquads.Instance().RemoveSquad(i);

                    GameStateManager.Instance().SetState(new Adventuring());

                    DialogUI.Instance.HideDialog();
                }
            }
        }
    }
}