using Assets.Scripts.Combat.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Bonuses
{
    [System.Serializable]
    abstract public class TemporaryBonus : Bonus, IMessageable
    {
        [SerializeField] protected int duration;
        [SerializeField] protected bool isCommited;
        [SerializeField] protected bool isRolledBack;

        protected TemporaryBonus(int duration, bool isStackable) : base(isStackable)
        {
            this.duration = duration;
            isCommited = false;
            isRolledBack = false;
        }

        public void Update()
        {
            if (!isCommited)
            {
                Commit();

                isCommited = true;
            }

            duration--;

            if (IsExpired())
            {
                if (!isRolledBack)
                {
                    Rollback();

                    isRolledBack = true;
                }
            }
        }

        public bool IsExpired()
        {
            return duration <= 0;
        }

        public bool IsCommited()
        {
            return isCommited;
        }

        public bool IsRolledBack()
        {
            return isRolledBack;
        }

        abstract protected void Commit();
        abstract protected void Rollback();
        public abstract string Message();
    }
}