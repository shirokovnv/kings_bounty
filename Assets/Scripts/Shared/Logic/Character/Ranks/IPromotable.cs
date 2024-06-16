namespace Assets.Scripts.Shared.Logic.Character.Ranks
{
    public interface IPromotable
    {
        public string GetPromotionName();
        public int CountOfVillainsToBeCaught();
        public int LeadershipBonus();
        public int KnowledgeBonus();
        public int SpellPowerBonus();
        public int WeekCommissionBonus();
    }
}