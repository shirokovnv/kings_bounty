using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Shared.Data.Managers;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Object
{
    [System.Serializable]
    public class Chest : BaseObject
    {
        public enum ChestType
        {
            Treasure,
            Commission,
            Spell,
            Spellpower,
            Knowledge,
            MapReveal,
            PathToContinent,
        }

        [SerializeField] private ChestType chestType;
        [SerializeField] private int chestValue;

        private Chest(string name) : base(name, ObjectType.chest)
        {
        }

        public void SetChestType(ChestType chestType)
        {
            this.chestType = chestType;
        }

        public ChestType GetChestType()
        {
            return chestType;
        }

        public int GetChestValue()
        {
            return chestValue;
        }

        public static Chest CreateRandom(int x, int y, int continentNumber, int chestValue)
        {
            int probability = Random.Range(0, 100);

            var chest = new Chest($"chest({x}#{y}#{continentNumber})")
            {
                X = x,
                Y = y,
                ContinentNumber = continentNumber,

                // by default it's a gold chest
                chestValue = chestValue,
                chestType = ChestType.Treasure
            };

            // chest increases royal salary
            if (probability < 10)
            {
                chest.chestType = ChestType.Commission;
            }

            // chest contains spell
            if (probability >= 10 && probability < 15)
            {
                chest.chestType = ChestType.Spell;
                chest.chestValue = SpellManager.Instance().GetRandomSpellIndex();
            }

            // chest increases spell power
            if (probability >= 15 && probability < 17)
            {
                chest.chestType = ChestType.Spellpower;
                chest.chestValue = Random.Range(1, continentNumber + 1);
            }

            // chest increases knowledge
            if (probability >= 17 && probability < 19)
            {
                chest.chestType = ChestType.Knowledge;
                chest.chestValue = Random.Range(1, continentNumber + 1);
            }

            return chest;
        }
    }
}