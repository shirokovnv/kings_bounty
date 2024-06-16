using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Adventure
{
    public class CastleGateSpell : ICastable
    {
        private readonly PortalTarget target;

        public CastleGateSpell(PortalTarget target)
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
            }

            if (Player.Instance().IsNavigating())
            {
                EventBus.Instance.PostEvent(new OnShore(target.X, target.Y));
            }

            EventBus.Instance.PostEvent(new OnCancelBoat());
            EventBus.Instance.PostEvent(new OnChangePosition(target.X, target.Y));

            DialogUI.Instance.UpdateTextMessage("The portal takes you to the castle.");
        }
    }
}