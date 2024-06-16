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
    public class Castle : BaseObject, ICombatable, ISquadable
    {
        private const int MAX_SQUADS = 5;

        [SerializeField] private List<UnitGroup> squads = new();
        [SerializeField] private int tileIndex = 4;
        [SerializeField] private int strength;
        [SerializeField] private int spoilsOfWar;
        [SerializeField] private int followers;
        [SerializeField] private CastleOwner owner;
        [SerializeField] private bool isContracted;
        [SerializeField] private bool isVisited;

        private Castle(string name) : base(name, ObjectType.castleGate)
        {
        }

        public int GetTileIndex()
        {
            return tileIndex;
        }

        public CastleOwner GetOwner()
        {
            return owner;
        }
        public void SetOwner(CastleOwner owner)
        {
            this.owner = owner;
        }

        public bool IsContracted() { return isContracted; }
        public void SetIsContracted(bool isContracted) { this.isContracted = isContracted; }

        public int GetStrength() { return strength; }

        public static Castle CreateRandom(int x, int y, int continentNumber, List<UnitScriptableObject> units, int strength)
        {
            var castle = new Castle($"castle({x}#{y}#{continentNumber})")
            {
                X = x,
                Y = y,
                ContinentNumber = continentNumber,
                isVisited = false,
                strength = strength,
                followers = 0,
            };

            castle.BuildGarrison(units, strength);

            return castle;
        }

        public bool IsVisited() { return isVisited; }
        public void SetVisited(bool isVisited) { this.isVisited = isVisited; }

        public BaseObject GetObject()
        {
            return this;
        }

        public int GetSpoilsOfWar()
        {
            return spoilsOfWar;
        }

        public void CalculateSpoilsOfWar()
        {
            spoilsOfWar = squads.Aggregate(0, (acc, squad) =>
            {
                return acc + squad.CurrentQuantity() * squad.Unit.Cost / ICombatable.SPOILS_OF_WAR_PENALTY;
            });
        }

        public List<UnitGroup> GetSquads() { return squads; }

        public void AddOrRefillSquad(UnitScriptableObject unit, int quantity, UnitGroup.UnitOwner owner)
        {
            var squad = squads.FirstOrDefault(s => s.Unit.Name == unit.Name);

            if (squads.Count >= MAX_SQUADS && squad == null)
            {
                return;
            }

            if (squad != null)
            {
                squad.SetQuantity(quantity + squad.CurrentQuantity());
            }
            else
            {
                squads.Add(new UnitGroup(unit, quantity, owner, 0, squads.Count));
            }
        }

        public void RemoveSquad(int index)
        {
            if (index < 0 || index >= squads.Count)
            {
                return;
            }

            squads.RemoveAt(index);
        }

        public void RemoveAllSquads()
        {
            squads.Clear();
        }

        public bool CanHireSquad(UnitScriptableObject unit)
        {
            var squad = squads.FirstOrDefault(s => s.Unit.Name == unit.Name);

            return squad != null || squads.Count < MAX_SQUADS;
        }

        public void BuildGarrison(List<UnitScriptableObject> units, int strength)
        {
            RemoveAllSquads();

            for (int i = 0; i < MAX_SQUADS; i++)
            {
                var unit = units[Random.Range(0, units.Count)];
                int quantity = strength / unit.HP;
                if (quantity == 0)
                {
                    quantity = 1;
                }

                AddOrRefillSquad(unit, quantity, UnitGroup.UnitOwner.opponent);
            }
        }
    }
}