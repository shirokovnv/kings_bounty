using Assets.Resources.ScriptableObjects;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Interfaces
{
    public interface ISquadable
    {
        public void AddOrRefillSquad(UnitScriptableObject unit, int quantity, UnitGroup.UnitOwner owner);
        public void RemoveSquad(int index);
        public List<UnitGroup> GetSquads();
        public void RemoveAllSquads();
        public bool CanHireSquad(UnitScriptableObject unit);
    }
}