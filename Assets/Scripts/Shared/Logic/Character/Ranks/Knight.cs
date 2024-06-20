using Assets.Scripts.Combat;
using Assets.Scripts.Shared.Data.Managers;
using System.Collections.Generic;

namespace Assets.Scripts.Shared.Logic.Character.Ranks
{
    public class Knight : Rank
    {
        public override List<UnitGroup> GetBaseArmy()
        {
            var militia = UnitManager.Instance().GetUnitByName("Militia");
            var archers = UnitManager.Instance().GetUnitByName("Archers");

            return new List<UnitGroup>
        {
            new(militia, 20, UnitGroup.UnitOwner.player, 0, 0),
            new(archers, 2, UnitGroup.UnitOwner.player, 0, 1),
        };
        }

        public override int GetBaseKnowledge()
        {
            return 2;
        }

        public override int GetBaseLeadership()
        {
            return 100;
        }

        public override BaseRank GetBaseRank()
        {
            return BaseRank.Knight;
        }

        public override int GetBaseSpellPower()
        {
            return 1;
        }

        public override int GetBaseWeekCommission()
        {
            return 1000;
        }

        public override int GetMovesPerDay()
        {
            return 35;
        }

        public override string GetRankName()
        {
            return "Knight";
        }
    }
}