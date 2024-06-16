using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Utility;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Object
{
    [System.Serializable]
    public class Dwelling : BaseObject, ISerializationCallbackReceiver
    {
        public enum DwellingType
        {
            plains,
            forest,
            dungeon,
            cavern,
            castle
        }

        [SerializeField] private DwellingType dwellingType;
        [SerializeField] private int currentPopulation;
        [SerializeField] private int unitID;

        [System.NonSerialized] public UnitScriptableObject Unit;

        private Dwelling(string name) : base(name, ObjectType.dwelling)
        {
        }

        public static Dwelling CreateVirtual(UnitScriptableObject unit)
        {
            var dwelling = new Dwelling($"dwelling#{unit.Name}")
            {
                Unit = unit,
                dwellingType = unit.DwellingType,
                currentPopulation = unit.Population,
            };

            return dwelling;
        }

        public static Dwelling Initialize(int x, int y, int continentNumber, int minLevel, int maxLevel)
        {
            var dwelling = new Dwelling($"dwelling({x}#{y}#{continentNumber})")
            {
                dwellingType = Utils.RandomEnumValueExcept(DwellingType.castle),
            };

            dwelling.Unit = UnitManager.Instance().GetRandomUnitWith(dwelling.dwellingType, minLevel, maxLevel);
            dwelling.currentPopulation = dwelling.Unit.Population;

            dwelling.X = x;
            dwelling.Y = y;
            dwelling.ContinentNumber = continentNumber;

            return dwelling;
        }

        public DwellingType GetDwellingType()
        {
            return dwellingType;
        }

        public int CurrentPopulation() { return currentPopulation; }
        public void SetCurrentPopulation(int population) { currentPopulation = population; }

        public void ChangePopulation(int amount)
        {
            currentPopulation += amount;
        }

        public void OnBeforeSerialize()
        {
            unitID = Unit.GetScriptID();
        }

        public void OnAfterDeserialize()
        {
            Unit = UnitManager.Instance().GetUnitByID(unitID);
        }
    }
}