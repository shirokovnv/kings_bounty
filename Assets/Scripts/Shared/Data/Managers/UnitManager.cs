using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Shared.Data.Managers
{
    public class UnitManager
    {
        private static UnitManager instance;
        private List<UnitScriptableObject> units;

        public static UnitManager Instance()
        {
            if (instance == null)
            {
                instance = new UnitManager();
                instance.units = UnityEngine.Resources.LoadAll<UnitScriptableObject>("ScriptableObjects").ToList();
            }

            return instance;
        }

        public List<UnitScriptableObject> GetUnits() { return units; }

        public UnitScriptableObject GetRandomUnit()
        {
            return units[Random.Range(0, units.Count)];
        }

        public UnitScriptableObject GetRandomUnitExceptType(Dwelling.DwellingType type)
        {
            var unitGroup = units.Where(unit => unit.DwellingType != type).ToList();

            return unitGroup[Random.Range(0, unitGroup.Count)];
        }

        public UnitScriptableObject GetRandomUnitWith(Dwelling.DwellingType dwellingType, int minLevel, int maxLevel)
        {
            var unitGroup = units
                .Where(unit => unit.DwellingType == dwellingType && unit.Level >= minLevel && unit.Level <= maxLevel)
                .ToList();
            return unitGroup[Random.Range(0, unitGroup.Count)];
        }

        public List<UnitScriptableObject> GetUnitsByType(Dwelling.DwellingType type)
        {
            return units.Where(unit => unit.DwellingType == type).ToList();
        }

        public UnitScriptableObject GetUnitByName(string name)
        {
            return units.Where(unit => unit.Name == name).FirstOrDefault();
        }

        public UnitScriptableObject GetUnitByID(int id)
        {
            return units.Where(unit => unit.GetScriptID() == id).FirstOrDefault();
        }
    }
}