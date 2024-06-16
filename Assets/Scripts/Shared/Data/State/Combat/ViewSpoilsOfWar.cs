using Assets.Scripts.Combat.Interfaces;

namespace Assets.Scripts.Shared.Data.State.Combat
{
    public class ViewSpoilsOfWar : InCombat
    {
        public ViewSpoilsOfWar(ICombatable combatable, int totalQuantity) : base(combatable, totalQuantity)
        {
        }
    }
}