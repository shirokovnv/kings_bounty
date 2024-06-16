namespace Assets.Scripts.Shared.Logic.Character.Ranks.Promotions
{
    public class Marshal : IPromotable
    {
        public int CountOfVillainsToBeCaught()
        {
            return 8;
        }

        public string GetPromotionName()
        {
            return "Marshal";
        }

        public int KnowledgeBonus()
        {
            return 4;
        }

        public int LeadershipBonus()
        {
            return 200;
        }

        public int SpellPowerBonus()
        {
            return 1;
        }

        public int WeekCommissionBonus()
        {
            return 2000;
        }
    }
}