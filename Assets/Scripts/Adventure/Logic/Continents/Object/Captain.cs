using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Combat;
using Assets.Scripts.Combat.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Object
{
    [System.Serializable]
    public class Captain : BaseObject, ICombatable
    {
        private const float LOYAL_PROBABILITY = 0.05f;

        [SerializeField] private List<UnitGroup> squads;
        [SerializeField] private bool isLoyal;
        [SerializeField] private int spoilsOfWar;
        [SerializeField] private int followers;

        private Captain(string name) : base(name, ObjectType.captain)
        {
        }

        public int GetSquadsCount()
        {
            return squads.Count;
        }

        public bool IsLoyal()
        {
            return isLoyal;
        }

        public List<UnitGroup> GetSquads()
        {
            return squads;
        }

        public void AddGroup(UnitGroup group)
        {
            squads.Add(group);
        }

        public static Captain CreateRandom(
            int x,
            int y,
            int continentNumber,
            List<UnitScriptableObject> units,
            int numGroups,
            int strength
            )
        {
            Captain captain = new($"captain({x}#{y}#{continentNumber})")
            {
                X = x,
                Y = y,
                followers = 0,
                ContinentNumber = continentNumber,
                isLoyal = false,
                squads = new(),
            };

            // first check loyalty
            float loyalProbability = Random.value;

            if (loyalProbability < LOYAL_PROBABILITY)
            {
                captain.isLoyal = true;
                numGroups = 1;
            }

            for (int i = 0; i < numGroups; i++)
            {
                var unit = units[Random.Range(0, units.Count)];
                int quantity = strength / unit.HP;
                if (quantity == 0)
                {
                    quantity = 1;
                }

                var unitGroup = new UnitGroup(unit, quantity, UnitGroup.UnitOwner.opponent, 5, i);

                captain.AddGroup(unitGroup);
            }

            return captain;
        }

        public BaseObject GetObject()
        {
            return this;
        }

        public void CalculateSpoilsOfWar()
        {
            spoilsOfWar = squads.Aggregate(0, (acc, squad) =>
            {
                return acc + squad.CurrentQuantity() * squad.Unit.Cost / ICombatable.SPOILS_OF_WAR_PENALTY;
            });
        }

        public int GetSpoilsOfWar()
        {
            return spoilsOfWar;
        }
    }
}