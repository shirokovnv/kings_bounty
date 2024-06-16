using Assets.Scripts.Combat;
using Assets.Scripts.Shared.Data.Managers;
using System.Collections.Generic;

namespace Assets.Scripts.Shared.Logic.Character.Ranks
{
    public class Ranger : Rank
    {
        public override List<UnitGroup> GetBaseArmy()
        {
            var wolves = UnitManager.Instance().GetUnitByName("Wolves");

            return new List<UnitGroup>
        {
            new(wolves, 10, UnitGroup.UnitOwner.player, 0, 1)
        };
        }

        public override int GetBaseKnowledge()
        {
            return 3;
        }

        public override int GetBaseLeadership()
        {
            return 80;
        }

        public override int GetBaseSpellPower()
        {
            return 2;
        }

        public override int GetBaseWeekCommission()
        {
            return 2000;
        }

        public override int GetMovesPerDay()
        {
            return 40;
        }

        public override string GetRankName()
        {
            return "Ranger";
        }
    }
}