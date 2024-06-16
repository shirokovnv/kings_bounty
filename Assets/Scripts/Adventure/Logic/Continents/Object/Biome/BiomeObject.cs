using Assets.Scripts.Adventure.Logic.Continents.Base;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Object.Biome
{
    [System.Serializable]
    public class BiomeObject : NamedObject
    {
        [SerializeField] private BiomeType type;
        // TODO: this const represents index in tile spritesheet. 12 is the "solid" tile.
        // It needs only for the View. Think about refactoring.
        [SerializeField] private int tileIndex = 12;

        public BiomeObject(string name, BiomeType type) : base(name)
        {
            this.type = type;
        }

        public void SetTileIndex(int tileIndex)
        {
            this.tileIndex = tileIndex;
        }

        public int GetTileIndex()
        {
            return tileIndex;
        }

        public BiomeType GetBiomeType() { return type; }
        public void SetBiomeType(BiomeType type) { this.type = type; }
    }
}