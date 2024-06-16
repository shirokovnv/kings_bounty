namespace Assets.Scripts.Shared.Logic.Character.Ranks.Promotions
{
    public class Hunter : IPromotable
    {
        public int CountOfVillainsToBeCaught()
        {
            return 2;
        }

        public string GetPromotionName()
        {
            return "Hunter";
        }

        public int KnowledgeBonus()
        {
            return 4;
        }

        public int LeadershipBonus()
        {
            return 80;
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