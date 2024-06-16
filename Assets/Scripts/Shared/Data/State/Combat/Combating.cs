using Assets.Scripts.Combat.Interfaces;

namespace Assets.Scripts.Shared.Data.State.Combat
{
    public class Combating : InCombat
    {
        public Combating(ICombatable combatable, int totalQuantity) : base(combatable, totalQuantity)
        {
        }
    }
}