using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects
{
    abstract public class PassableObjectInteractor : BaseObjectInteractor
    {
        protected void MoveToObject(int oX, int oY)
        {
            bool isNavigating = Player.Instance().IsNavigating();

            TimeSystem.Instance().Tick();

            // check shore
            if (
                isNavigating
                )
            {
                object[] events = { new OnShore(oX, oY), new OnChangePosition(oX, oY) };
                EventBus.Instance.PostEventGroup(events);

                return;
            }

            EventBus.Instance.PostEvent(new OnChangePosition(oX, oY));
        }
    }
}