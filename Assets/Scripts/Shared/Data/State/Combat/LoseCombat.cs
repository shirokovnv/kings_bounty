using Assets.Scripts.Combat.Interfaces;

namespace Assets.Scripts.Shared.Data.State.Combat
{
    public class LoseCombat : InCombat
    {
        public LoseCombat(ICombatable combatable, int totalQuantity) : base(combatable, totalQuantity)
        {
        }
    }
}