using Assets.Scripts.Combat;
using System.Collections.Generic;

namespace Assets.Scripts.Shared.Logic.Character.Ranks
{
    public interface IRankable
    {
        public string GetRankName();
        public int GetBaseLeadership();
        public int GetBaseKnowledge();
        public int GetBaseSpellPower();
        public int GetBaseWeekCommission();
        public int GetMovesPerDay();
        public List<UnitGroup> GetBaseArmy();
    }
}