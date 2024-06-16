using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Shared.Data.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Object
{
    [System.Serializable]
    public class City : BaseObject, ISerializationCallbackReceiver
    {
        private SpellScriptableObject spell;
        [SerializeField] private int spellID;
        [SerializeField] private List<Vector2Int> waterSiblings;
        [SerializeField] private List<Vector2Int> grassSiblings;
        [SerializeField] private bool isVisited;

        // Link castle and city in Continent class
        [System.NonSerialized] private Object.Castle linkedCastle;

        private City(string name) : base(name, ObjectType.city)
        {
        }

        public SpellScriptableObject Spell { get { return spell; } }

        public Castle GetLinkedCastle()
        {
            return linkedCastle;
        }

        public void SetLinkedCastle(Castle value)
        {
            linkedCastle = value;
        }

        public static City Create(int x, int y, int continentNumber, SpellScriptableObject spell)
        {
            var city = new City($"city({x}#{y}#{continentNumber})")
            {
                X = x,
                Y = y,
                ContinentNumber = continentNumber,
                spell = spell,
                waterSiblings = new List<Vector2Int>(),
                isVisited = false
            };

            return city;
        }

        public List<Vector2Int> GetWaterSiblings() { return waterSiblings; }
        public void SetWaterSiblings(List<Vector2Int> waterSiblings)
        {
            this.waterSiblings = waterSiblings;
        }

        public List<Vector2Int> GetGrassSiblings()
        {
            return grassSiblings;
        }
        public void SetGrassSiblings(List<Vector2Int> grassSiblings)
        {
            this.grassSiblings = grassSiblings;
        }

        public bool IsVisited() { return isVisited; }
        public void SetVisited(bool isVisited) { this.isVisited = isVisited; }

        public bool CanLaunchBoat()
        {
            return waterSiblings.Count > 0;
        }

        public Vector2Int LaunchTheBoat(int pX, int pY)
        {
            if (waterSiblings.Count == 0)
            {
                return new Vector2Int(-1, -1);
            }

            waterSiblings.Sort(delegate (Vector2Int one, Vector2Int two)
            {
                int oneDist = (pX - one.x) * (pX - one.x) + (pY - one.y) * (pY - one.y);
                int twoDist = (pX - two.x) * (pX - two.x) + (pY - two.y) * (pY - two.y);

                if (oneDist < twoDist)
                {
                    return -1;
                }

                if (oneDist > twoDist)
                {
                    return 1;
                }

                return 0;
            });

            Vector2Int position = waterSiblings[0];

            return position;
        }

        public void OnBeforeSerialize()
        {
            spellID = spell.GetScriptID();
        }

        public void OnAfterDeserialize()
        {
            spell = SpellManager.Instance().GetSpellByID(spellID);
        }
    }
}