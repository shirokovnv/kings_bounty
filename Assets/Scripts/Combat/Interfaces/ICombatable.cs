using Assets.Scripts.Adventure.Logic.Continents.Base;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Interfaces
{
    public interface ICombatable
    {
        public const int SPOILS_OF_WAR_PENALTY = 20;
        public const int LEADERSHIP_PENALTY = 50;
        public const float GROW_COEFFICIENT = 0.1f;

        public List<UnitGroup> GetSquads();
        public BaseObject GetObject();
        public void CalculateSpoilsOfWar();
        public int GetSpoilsOfWar();
        public void CalculateLeadership();
        public int GetLeadership();
        public void GrowInNumbers();
    }
}