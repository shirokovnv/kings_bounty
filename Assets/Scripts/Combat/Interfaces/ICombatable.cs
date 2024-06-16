using Assets.Scripts.Adventure.Logic.Continents.Base;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Interfaces
{
    public interface ICombatable
    {
        public const int SPOILS_OF_WAR_PENALTY = 20;

        public List<UnitGroup> GetSquads();
        public BaseObject GetObject();
        public void CalculateSpoilsOfWar();
        public int GetSpoilsOfWar();
    }
}