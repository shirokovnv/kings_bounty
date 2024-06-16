namespace Assets.Scripts.Shared.Logic.Character.Ranks
{
    [System.Serializable]
    public struct RankPackage
    {
        public string BaseType;
        public int PromotedTimes;

        public readonly Rank GetRank() => BaseType switch
        {
            nameof(Knight) => new Knight(),
            nameof(Sorceress) => new Sorceress(),
            nameof(Ranger) => new Ranger(),
            _ => throw new System.Exception("Unknown Rank."),
        };
    }
}