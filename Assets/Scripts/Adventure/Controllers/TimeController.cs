using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Data.State.Adventure;
using UnityEngine;

namespace Assets.Scripts.Adventure.Controllers
{
    public class TimeController : MonoBehaviour
    {
        private void Awake()
        {
            EventBus.Instance.Register(this);
        }

        public void OnEvent(OnWeekEnd e)
        {
            GameStateManager.Instance().SetState(new ViewWeekEnd
            {
                WeekNumber = e.WeekNumber,
                AfterSpentCallback = e.AfterSpentCallback
            });
        }
    }
}