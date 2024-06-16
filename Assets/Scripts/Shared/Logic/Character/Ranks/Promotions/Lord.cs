namespace Assets.Scripts.Shared.Logic.Character.Ranks.Promotions
{
    public class Lord : IPromotable
    {
        public int CountOfVillainsToBeCaught()
        {
            return 14;
        }

        public string GetPromotionName()
        {
            return "Lord";
        }

        public int KnowledgeBonus()
        {
            return 5;
        }

        public int LeadershipBonus()
        {
            return 500;
        }

        public int SpellPowerBonus()
        {
            return 2;
        }

        public int WeekCommissionBonus()
        {
            return 4000;
        }
    }
}