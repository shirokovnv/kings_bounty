using Assets.Scripts.Combat;
using Assets.Scripts.Shared.Data.Managers;
using System.Collections.Generic;

namespace Assets.Scripts.Shared.Logic.Character.Ranks
{
    public class Sorceress : Rank
    {
        public override List<UnitGroup> GetBaseArmy()
        {
            var peasants = UnitManager.Instance().GetUnitByName("Peasants");
            var sprites = UnitManager.Instance().GetUnitByName("Sprites");

            return new List<UnitGroup>
        {
            new(peasants, 20, UnitGroup.UnitOwner.player, 0, 0),
            new(sprites, 10, UnitGroup.UnitOwner.player, 0, 1)
        };
        }

        public override int GetBaseKnowledge()
        {
            return 5;
        }

        public override int GetBaseLeadership()
        {
            return 60;
        }

        public override BaseRank GetBaseRank()
        {
            return BaseRank.Sorceress;
        }

        public override int GetBaseSpellPower()
        {
            return 3;
        }

        public override int GetBaseWeekCommission()
        {
            return 3000;
        }

        public override int GetMovesPerDay()
        {
            return 30;
        }

        public override string GetRankName()
        {
            return "Sorceress";
        }
    }
}