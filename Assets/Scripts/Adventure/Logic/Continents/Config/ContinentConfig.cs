using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Config
{
    [System.Serializable]
    public class ContinentConfig
    {
        private const int DEFAULT_WIDTH = 64;
        private const int DEFAULT_HEIGHT = 64;

        private const int DEFAULT_WATER_EDGE = 5;
        private const int DEFAULT_SAND_RING_EDGE = 0;

        private const int DEFAULT_MAX_CONTINENTS_COUNT = 4;

        [SerializeField] private BiomeConfig biomeConfig;
        [SerializeField] private ObjectConfig objectConfig;
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private int waterEdge;
        [SerializeField] private int sandRingEdge;
        [SerializeField] private int number;
        [SerializeField] private int maxContinentsCount;

        public ContinentConfig(
            int width = DEFAULT_WIDTH,
            int height = DEFAULT_HEIGHT,
            int waterEdge = DEFAULT_WATER_EDGE,
            int sandRingEdge = DEFAULT_SAND_RING_EDGE,
            BiomeConfig biomeConfig = null,
            ObjectConfig objectConfig = null,
            int maxContinentsCount = DEFAULT_MAX_CONTINENTS_COUNT
        )
        {
            this.width = width;
            this.height = height;
            this.waterEdge = waterEdge;
            this.sandRingEdge = sandRingEdge;

            this.biomeConfig = biomeConfig ?? new BiomeConfig();
            this.objectConfig = objectConfig ?? new ObjectConfig();

            this.maxContinentsCount = maxContinentsCount;
        }

        public int GetMaxContinentsCount()
        {
            return maxContinentsCount;
        }

        public void SetMaxContinentsCount(int maxContinentsCount)
        {
            this.maxContinentsCount = maxContinentsCount;
        }

        public BiomeConfig GetBiomeConfig()
        {
            return biomeConfig;
        }

        public void SetBiomeConfig(BiomeConfig value)
        {
            biomeConfig = value;
        }

        public ObjectConfig GetObjectConfig()
        {
            return objectConfig;
        }

        public void SetObjectConfig(ObjectConfig value)
        {
            objectConfig = value;
        }

        public int GetWidth()
        {
            return width;
        }

        public void SetWidth(int value)
        {
            width = value;
        }

        public int GetHeight()
        {
            return height;
        }

        public void SetHeight(int value)
        {
            height = value;
        }

        public int GetWaterEdge()
        {
            return waterEdge;
        }

        public void SetWaterEdge(int value)
        {
            waterEdge = value;
        }

        public int GetSandRingEdge()
        {
            return sandRingEdge;
        }

        public void SetSandRingEdge(int value)
        {
            sandRingEdge = value;
        }

        public int GetNumber()
        {
            return number;
        }

        public void SetNumber(int value)
        {
            number = value;
        }
    }
}