using Assets.Scripts.Shared.Logic.Bonuses.Modifiers;
using Assets.Scripts.Shared.Logic.Bonuses.Temporary;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Bonuses
{
    [System.Serializable]
    public struct BonusPackage
    {
        public string Type;
        public string Content;

        public BonusPackage(Bonus bonus)
        {
            Content = JsonUtility.ToJson(bonus);
            Type = bonus.GetType().ToString();
        }

        public readonly Bonus GetBonus()
        {
            return Type switch
            {
                nameof(CharismaBonus) => JsonUtility.FromJson<CharismaBonus>(Content),
                nameof(StandStillBonus) => JsonUtility.FromJson<StandStillBonus>(Content),
                nameof(AttackModifier) => JsonUtility.FromJson<AttackModifier>(Content),
                nameof(DefenceModifier) => JsonUtility.FromJson<DefenceModifier>(Content),
                _ => throw new System.Exception("Unknown bonus type."),
            };
        }
    }
}