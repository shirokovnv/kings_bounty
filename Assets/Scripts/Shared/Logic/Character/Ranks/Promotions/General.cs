namespace Assets.Scripts.Shared.Logic.Character.Ranks.Promotions
{
    public class General : IPromotable
    {
        public int CountOfVillainsToBeCaught()
        {
            return 2;
        }

        public string GetPromotionName()
        {
            return "General";
        }

        public int KnowledgeBonus()
        {
            return 3;
        }

        public int LeadershipBonus()
        {
            return 100;
        }

        public int SpellPowerBonus()
        {
            return 1;
        }

        public int WeekCommissionBonus()
        {
            return 1000;
        }
    }
}