using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Data.State.Adventure;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Dialogs
{
    public class PuzzleMapInteractor : BaseDialogInteractor<Adventuring, ViewPuzzleMap>
    {
        public override void WaitForStart()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameStateManager.Instance().SetState(new ViewPuzzleMap());

                EventBus.Instance.PostEvent(new OnViewPuzzleMap());
            }
        }

        public override void WaitForFinish()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance().SetState(new Adventuring());

                EventBus.Instance.PostEvent(new OnDidViewPuzzleMap());
            }
        }
    }
}