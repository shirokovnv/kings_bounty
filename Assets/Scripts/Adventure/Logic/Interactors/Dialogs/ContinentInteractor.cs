using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using System;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Dialogs
{
    public class ContinentInteractor : BaseDialogInteractor<Adventuring, Navigating>
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
            if (Input.GetKeyDown(KeyCode.N) && Player.Instance().IsNavigating())
            {
                var revealedContinents = ContinentSystem.Instance().GetRevealedContinents();

                string title = "Choose continent";
                string text = string.Empty;

                for (int i = 0; i < revealedContinents.Count; i++)
                {
                    text += char.ToUpper(Convert.ToChar(keyCodes[i])) + ") " + revealedContinents[i].GetName() + "\r\n";
                }

                DialogUI.Instance.ShowDialogUI(title, text, null);

                GameStateManager.Instance().SetState(new Navigating());
            }
        }

        public override void WaitForFinish()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance().SetState(new Adventuring());

                DialogUI.Instance.HideDialog();
            }

            for (int i = 0; i < keyCodes.Length; i++)
            {
                var continent = ContinentSystem.Instance().GetContinentAtNumber(i + 1);

                if (Input.GetKeyDown(keyCodes[i]) &&
                    continent != null &&
                    continent.IsRevealed())
                {
                    DialogUI.Instance.HideDialog();

                    EventBus.Instance.PostEvent(new OnChangeContinent(i + 1));
                    EventBus.Instance.PostEvent(new OnChangePosition(3, 3));

                    GameStateManager.Instance().SetState(new Adventuring());

                    TimeSystem.Instance().WeekEnd();
                }
            }
        }
    }
}