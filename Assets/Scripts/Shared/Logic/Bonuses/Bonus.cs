using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Bonuses
{
    [System.Serializable]
    abstract public class Bonus
    {
        [SerializeField] protected bool isStackable;

        protected Bonus(bool isStackable)
        {
            this.isStackable = isStackable;
        }

        public bool IsStackable() { return isStackable; }
    }
}