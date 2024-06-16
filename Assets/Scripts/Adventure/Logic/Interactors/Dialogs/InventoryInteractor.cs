using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Data.State.Adventure;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Dialogs
{
    public class InventoryInteractor : BaseDialogInteractor<Adventuring, ViewInventory>
    {
        public override void WaitForStart()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                EventBus.Instance.PostEvent(new OnViewInventory());

                GameStateManager.Instance().SetState(
                    new ViewInventory()
                    );
            }
        }

        public override void WaitForFinish()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EventBus.Instance.PostEvent(new OnHideInventory());

                GameStateManager.Instance().SetState(
                            new Adventuring()
                );
            }
        }
    }
}