namespace Assets.Scripts.Shared.Logic.Character.Ranks.Promotions
{
    public class ArchMage : IPromotable
    {
        public int CountOfVillainsToBeCaught()
        {
            return 12;
        }

        public string GetPromotionName()
        {
            return "Archmage";
        }

        public int KnowledgeBonus()
        {
            return 12;
        }

        public int LeadershipBonus()
        {
            return 300;
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