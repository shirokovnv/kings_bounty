using Assets.Scripts.Shared.Logic.Bonuses.Modifiers;
using Assets.Scripts.Shared.Logic.Bonuses.Temporary;
using System;
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
            if (!Enum.TryParse(Type, out BonusEnum bonusEnum))
            {
                throw new Exception("Unknown bonus.");
            }

            return bonusEnum switch
            {
                BonusEnum.CharismaBonus => JsonUtility.FromJson<CharismaBonus>(Content),
                BonusEnum.StandStillBonus => JsonUtility.FromJson<StandStillBonus>(Content),
                BonusEnum.WalkModeBonus => JsonUtility.FromJson<WalkModeBonus>(Content),

                BonusEnum.AttackModifier => JsonUtility.FromJson<AttackModifier>(Content),
                BonusEnum.DefenceModifier => JsonUtility.FromJson<DefenceModifier>(Content),
                
                _ => null,
            };
        }
    }
}