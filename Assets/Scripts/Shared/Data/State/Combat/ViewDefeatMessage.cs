using Assets.Scripts.Combat.Interfaces;

namespace Assets.Scripts.Shared.Data.State.Combat
{
    public class ViewDefeatMessage : InCombat
    {
        public ViewDefeatMessage(ICombatable combatable, int totalQuantity) : base(combatable, totalQuantity)
        {
        }
    }
}