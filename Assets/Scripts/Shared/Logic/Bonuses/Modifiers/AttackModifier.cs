using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Bonuses.Modifiers
{
    [System.Serializable]
    public class AttackModifier : Modifier<float>
    {
        [SerializeField] protected float value;

        public AttackModifier(float value, bool isStackable) : base(isStackable)
        {
            this.value = value;
        }

        public override BonusEnum GetBonusEnumValue()
        {
            return BonusEnum.AttackModifier;
        }

        public override float GetValue()
        {
            return value;
        }
    }
}