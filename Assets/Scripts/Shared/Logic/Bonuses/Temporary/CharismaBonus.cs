using Assets.Scripts.Shared.Logic.Character;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Bonuses.Temporary
{
    [System.Serializable]
    public class CharismaBonus : TemporaryBonus
    {
        [SerializeField] protected int bonus;

        public CharismaBonus(int duration, int bonus, bool isStackable)
            : base(duration, isStackable)
        {
            this.bonus = bonus;
        }

        public override BonusEnum GetBonusEnumValue()
        {
            return BonusEnum.CharismaBonus;
        }

        public override string Message()
        {
            return $"CHARISMA ({bonus}): {duration}";
        }

        protected override void Commit()
        {
            PlayerStats.Instance().ChangeLeadership(bonus);
        }

        protected override void Rollback()
        {
            PlayerStats.Instance().ChangeLeadership(-bonus);
        }
    }
}