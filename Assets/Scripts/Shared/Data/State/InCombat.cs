using Assets.Scripts.Combat.Interfaces;

namespace Assets.Scripts.Shared.Data.State
{
    abstract public class InCombat : GameState
    {
        protected ICombatable combatable;
        protected int totalQuantity;

        public InCombat(ICombatable combatable, int totalQuantity)
        {
            this.combatable = combatable;
            this.totalQuantity = totalQuantity;
        }

        public ICombatable GetCombatable()
        {
            return combatable;
        }

        public int GetTotalQuantity()
        {
            return totalQuantity;
        }
    }
}