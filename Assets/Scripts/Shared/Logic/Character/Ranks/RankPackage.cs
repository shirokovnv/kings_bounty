using System;

namespace Assets.Scripts.Shared.Logic.Character.Ranks
{
    [System.Serializable]
    public struct RankPackage
    {
        public string BaseType;
        public int PromotedTimes;

        public readonly Rank GetRank()
        {
            if (!Enum.TryParse(BaseType, out BaseRank baseRank))
            {
                throw new Exception("Unknown player rank.");
            }

            return baseRank switch
            {
                BaseRank.Knight => new Knight(),
                BaseRank.Ranger => new Ranger(),
                BaseRank.Sorceress => new Sorceress(),
                _ => null,
            };
        }
    }
}