namespace Assets.Scripts.Shared.Logic.Character.Ranks.Promotions
{
    public class Guardian : IPromotable
    {
        public int CountOfVillainsToBeCaught()
        {
            return 13;
        }

        public string GetPromotionName()
        {
            return "Guardian";
        }

        public int KnowledgeBonus()
        {
            return 6;
        }

        public int LeadershipBonus()
        {
            return 400;
        }

        public int SpellPowerBonus()
        {
            return 2;
        }

        public int WeekCommissionBonus()
        {
            return 2000;
        }
    }
}