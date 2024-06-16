namespace Assets.Scripts.Shared.Logic.Character.Ranks.Promotions
{
    public class Mage : IPromotable
    {
        public int CountOfVillainsToBeCaught()
        {
            return 6;
        }

        public string GetPromotionName()
        {
            return "Mage";
        }

        public int KnowledgeBonus()
        {
            return 10;
        }

        public int LeadershipBonus()
        {
            return 180;
        }

        public int SpellPowerBonus()
        {
            return 5;
        }

        public int WeekCommissionBonus()
        {
            return 1000;
        }
    }
}