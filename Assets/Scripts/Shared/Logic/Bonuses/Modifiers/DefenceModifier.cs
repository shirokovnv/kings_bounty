using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Bonuses.Modifiers
{
    [System.Serializable]
    public class DefenceModifier : Modifier<float>
    {
        [SerializeField] protected float value;

        public DefenceModifier(float value, bool isStackable) : base(isStackable)
        {
            this.value = value;
        }

        public override BonusEnum GetBonusEnumValue()
        {
            return BonusEnum.DefenceModifier;
        }

        public override float GetValue()
        {
            return value;
        }
    }
}