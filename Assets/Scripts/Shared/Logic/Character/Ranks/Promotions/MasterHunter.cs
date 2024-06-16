namespace Assets.Scripts.Shared.Logic.Character.Ranks.Promotions
{
    public class MasterHunter : IPromotable
    {
        public int CountOfVillainsToBeCaught()
        {
            return 7;
        }

        public string GetPromotionName()
        {
            return "Master hunter";
        }

        public int KnowledgeBonus()
        {
            return 5;
        }

        public int LeadershipBonus()
        {
            return 240;
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