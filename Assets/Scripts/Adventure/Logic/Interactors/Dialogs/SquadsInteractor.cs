using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Combat.Logic.Systems;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Dialogs
{
    public class SquadsInteractor : BaseDialogInteractor<Adventuring, ViewSquads>
    {
        public override void WaitForStart()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoraleSystem.Instance().CalculateMorale(PlayerSquads.Instance().GetSquads());
                GameStateManager.Instance().SetState(new ViewSquads());

                EventBus.Instance.PostEvent(new OnViewSquads());
            }
        }

        public override void WaitForFinish()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance().SetState(new Adventuring());

                EventBus.Instance.PostEvent(new OnHideSquads());
            }
        }
    }
}