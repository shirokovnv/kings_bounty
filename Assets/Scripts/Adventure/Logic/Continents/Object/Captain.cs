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

        [SerializeField] private int strength;
        [SerializeField] private List<UnitGroup> squads;
        [SerializeField] private bool isLoyal;
        private int spoilsOfWar;
        private int leadership;

        private Captain(string name) : base(name, ObjectType.captain)
        {
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
                ContinentNumber = continentNumber,
                isLoyal = false,
                squads = new(),
                leadership = 0,
                strength = strength
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

                captain.AddSquad(unit, quantity);
            }

            return captain;
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

        protected void AddSquad(UnitScriptableObject unit, int quantity)
        {
            squads.Add(new UnitGroup(unit, quantity, UnitGroup.UnitOwner.opponent, 0, squads.Count));
        }

        public void AddOrRefillSquad(UnitScriptableObject unit, int quantity, UnitGroup.UnitOwner owner)
        {
            var squad = squads.FirstOrDefault(s => s.Unit.Name == unit.Name);

            if (squad != null)
            {
                squad.SetQuantity(quantity + squad.CurrentQuantity());
            }
            else
            {
                squads.Add(new UnitGroup(unit, quantity, owner, 0, squads.Count));
            }
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

        public void CalculateLeadership()
        {
            leadership = squads.Aggregate(0, (acc, squad) =>
            {
                return acc + squad.CurrentQuantity() * squad.Unit.HP / ICombatable.LEADERSHIP_PENALTY;
            });
        }

        public int GetLeadership()
        {
            return leadership;
        }

        public void GrowInNumbers()
        {
            squads.ForEach(s =>
            {
                var unit = s.Unit;
                int growQuantity = Mathf.FloorToInt(strength * ICombatable.GROW_COEFFICIENT / unit.HP);

                AddOrRefillSquad(unit, growQuantity, UnitGroup.UnitOwner.opponent);
            });
        }
    }
}