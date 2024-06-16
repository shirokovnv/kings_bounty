using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Adventure
{
    public class TownGateSpell : ICastable
    {
        private readonly PortalTarget target;

        public TownGateSpell(PortalTarget target)
        {
            this.target = target;
        }

        public void Cast()
        {
            if (target == null)
            {
                DialogUI.Instance.UpdateTextMessage("Unable to portal.");

                return;
            }

            var currentContinentNumber = Player.Instance().ContinentNumber;

            if (currentContinentNumber != target.ContinentNumber)
            {
                EventBus.Instance.PostEvent(new OnChangeContinent(target.ContinentNumber));

                if (Boat.Instance().IsActive)
                {
                    EventBus.Instance.PostEvent(new OnCancelBoat());
                }
            }

            EventBus.Instance.PostEvent(new OnChangePosition(target.X, target.Y));

            DialogUI.Instance.UpdateTextMessage("The portal takes you to the town.");
        }
    }
}