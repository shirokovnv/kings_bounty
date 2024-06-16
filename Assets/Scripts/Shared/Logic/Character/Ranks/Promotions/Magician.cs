namespace Assets.Scripts.Shared.Logic.Character.Ranks.Promotions
{
    public class Magician : IPromotable
    {
        public int CountOfVillainsToBeCaught()
        {
            return 3;
        }

        public string GetPromotionName()
        {
            return "Magician";
        }

        public int KnowledgeBonus()
        {
            return 8;
        }

        public int LeadershipBonus()
        {
            return 60;
        }

        public int SpellPowerBonus()
        {
            return 3;
        }

        public int WeekCommissionBonus()
        {
            return 1000;
        }
    }
}