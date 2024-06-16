using Assets.Scripts.Adventure.Logic.Continents.Object.Biome;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Config
{
    [System.Serializable]
    public class BiomeConfig
    {
        private const int DEFAULT_WATER_LENGTH = 150;
        private const float DEFAULT_WATER_CONTINUITY = 0.7f;
        private const int DEFAULT_SAND_LENGTH = 100;
        private const float DEFAULT_SAND_CONTINUITY = 1.0f;

        [SerializeField] private int waterLength;
        [SerializeField] private float waterContinuity;

        [SerializeField] private KeyValuePair<BiomeType, float> mainBiome;
        [SerializeField] private KeyValuePair<BiomeType, float> subBiome;

        [SerializeField] private int sandLength;
        [SerializeField] private float sandContinuity;

        public BiomeConfig(
            int waterLength = DEFAULT_WATER_LENGTH,
            float waterContinuity = DEFAULT_WATER_CONTINUITY,
            int sandLength = DEFAULT_SAND_LENGTH,
            float sandContinuity = DEFAULT_SAND_CONTINUITY
            )
        {
            this.waterLength = waterLength;
            this.waterContinuity = waterContinuity;

            this.sandLength = sandLength;
            this.sandContinuity = sandContinuity;
        }

        public KeyValuePair<BiomeType, float> GetMainBiome()
        {
            return mainBiome;
        }

        public BiomeConfig SetMainBiome(KeyValuePair<BiomeType, float> biome)
        {
            mainBiome = biome;
            return this;
        }

        public KeyValuePair<BiomeType, float> GetSubBiome()
        {
            return subBiome;
        }

        public BiomeConfig SetSubBiome(KeyValuePair<BiomeType, float> biome)
        {
            subBiome = biome;
            return this;
        }

        public int GetWaterLength()
        {
            return waterLength;
        }

        public void SetWaterLength(int value)
        {
            waterLength = value;
        }

        public float GetWaterContinuity()
        {
            return waterContinuity;
        }

        public void SetWaterContinuity(float value)
        {
            waterContinuity = value;
        }

        public int GetSandLength()
        {
            return sandLength;
        }

        public void SetSandLength(int value)
        {
            sandLength = value;
        }

        public float GetSandContinuity()
        {
            return sandContinuity;
        }

        public void SetSandContinuity(float value)
        {
            sandContinuity = value;
        }
    }
}