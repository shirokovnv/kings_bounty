using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Character.Ranks.Promotions
{
    [System.Serializable]
    public class PromotionWrapper
    {
        [SerializeField] private List<IPromotable> promotions;
        [SerializeField] private int promotionIndex;

        private PromotionWrapper()
        {
            promotionIndex = -1;
        }

        public static PromotionWrapper Create(IRankable rank)
        {
            var promotionWrapper = new PromotionWrapper();

            switch (rank)
            {
                case Knight:
                    promotionWrapper.promotions = new List<IPromotable>
                {
                    new General(),
                    new Marshal(),
                    new Lord(),
                };

                    break;

                case Ranger:
                    promotionWrapper.promotions = new List<IPromotable>
                {
                    new Hunter(),
                    new MasterHunter(),
                    new Guardian(),
                };

                    break;

                case Sorceress:
                    promotionWrapper.promotions = new List<IPromotable>
                {
                    new Magician(),
                    new Mage(),
                    new ArchMage(),
                };

                    break;
            }

            return promotionWrapper;
        }

        public int PromotedTimes()
        {
            return promotionIndex + 1;
        }

        public bool HasNextPromotion()
        {
            return promotionIndex + 1 >= 0 && promotionIndex + 1 < promotions.Count;
        }

        public IPromotable NextPromotion()
        {
            if (HasNextPromotion())
            {
                return promotions.ElementAt(promotionIndex + 1);
            }

            return null;
        }

        public IPromotable CurrentPromotion()
        {
            if (promotionIndex < 0 || promotionIndex > promotions.Count)
            {
                return null;
            }

            return promotions.ElementAt(promotionIndex);
        }

        public void Promote()
        {
            if (HasNextPromotion())
            {
                promotionIndex++;
            }
        }
    }
}