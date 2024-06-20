using Assets.Scripts.Combat;
using Assets.Scripts.Shared.Logic.Character.Ranks.Promotions;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Character.Ranks
{
    abstract public class Rank : IRankable
    {
        [SerializeField] private PromotionWrapper promotions;

        public abstract int GetBaseKnowledge();
        public abstract int GetBaseLeadership();
        public abstract int GetBaseSpellPower();
        public abstract int GetBaseWeekCommission();
        public abstract string GetRankName();
        public abstract int GetMovesPerDay();
        public abstract List<UnitGroup> GetBaseArmy();
        public PromotionWrapper GetPromotions()
        {
            promotions ??= PromotionWrapper.Create(this);

            return promotions;
        }

        public string GetFullRankName()
        {
            return GetPromotions()
                .CurrentPromotion()
                .GetPromotionName() ?? GetRankName();
        }

        abstract public BaseRank GetBaseRank();
    }
}