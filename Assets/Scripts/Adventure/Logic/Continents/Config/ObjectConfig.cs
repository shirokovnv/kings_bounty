using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Config
{
    [System.Serializable]
    public class ObjectConfig
    {
        private const float DEFAULT_CHEST_FREQUENCY = 0.07f;
        private const float DEFAULT_SIGN_FREQUENCY = 0.05f;

        private const int DEFAULT_MIN_CHEST_STRENGTH = 100;
        private const int DEFAULT_MAX_CHEST_STRENGTH = 1000;

        private const int DEFAULT_MIN_CAPTAINS_COUNT = 40;
        private const int DEFAULT_MAX_CAPTAINS_COUNT = 60;

        private const int DEFAULT_MIN_CAPTAINS_GROUPS = 2;
        private const int DEFAULT_MAX_CAPTAINS_GROUPS = 5;

        private const int DEFAULT_MIN_CAPTAIN_STRENGTH = 1000;
        private const int DEFAULT_MAX_CAPTAIN_STRENGTH = 5000;

        private const int DEFAULT_CITIES_COUNT = 5;
        private const int DEFAULT_CASTLES_COUNT = 5;
        private const int DEFAULT_CASTLES_STRENGTH_BONUS = 500;

        private const int DEFAULT_MIN_DWELLINGS_COUNT = 5;
        private const int DEFAULT_MAX_DWELLINGS_COUNT = 10;

        private const int DEFAULT_MIN_DWELLING_CREATURE_LEVEL = 1;
        private const int DEFAULT_MAX_DWELLING_CREATURE_LEVEL = 5;

        [SerializeField] private float chestFrequency;
        [SerializeField] private float signFrequency;

        [SerializeField] private int minChestStrength;
        [SerializeField] private int maxChestStrength;

        [SerializeField] private int minCaptainsCount;
        [SerializeField] private int maxCaptainsCount;

        [SerializeField] private int minCaptainsGroups;
        [SerializeField] private int maxCaptainsGroups;

        [SerializeField] private int minCaptainStrength;
        [SerializeField] private int maxCaptainStrength;

        [SerializeField] private int citiesCount;
        [SerializeField] private int castlesCount;
        [SerializeField] private int castleStrengthBonus;

        [SerializeField] private int minDwellingsCount;
        [SerializeField] private int maxDwellingsCount;

        [SerializeField] private int minDwellingCreatureLevel;
        [SerializeField] private int maxDwellingCreatureLevel;

        public ObjectConfig()
        {
            chestFrequency = DEFAULT_CHEST_FREQUENCY;
            signFrequency = DEFAULT_SIGN_FREQUENCY;

            minCaptainsCount = DEFAULT_MIN_CAPTAINS_COUNT;
            maxCaptainsCount = DEFAULT_MAX_CAPTAINS_COUNT;

            minCaptainsGroups = DEFAULT_MIN_CAPTAINS_GROUPS;
            maxCaptainsGroups = DEFAULT_MAX_CAPTAINS_GROUPS;

            minCaptainStrength = DEFAULT_MIN_CAPTAIN_STRENGTH;
            maxCaptainStrength = DEFAULT_MAX_CAPTAIN_STRENGTH;

            citiesCount = DEFAULT_CITIES_COUNT;
            castlesCount = DEFAULT_CASTLES_COUNT;

            minChestStrength = DEFAULT_MIN_CHEST_STRENGTH;
            maxChestStrength = DEFAULT_MAX_CHEST_STRENGTH;

            minDwellingsCount = DEFAULT_MIN_DWELLINGS_COUNT;
            maxDwellingsCount = DEFAULT_MAX_DWELLINGS_COUNT;

            minDwellingCreatureLevel = DEFAULT_MIN_DWELLING_CREATURE_LEVEL;
            maxDwellingCreatureLevel = DEFAULT_MAX_DWELLING_CREATURE_LEVEL;
        }

        public float GetChestFrequency()
        {
            return chestFrequency;
        }
        public void SetChestFrequency(float value)
        {
            chestFrequency = value;
        }

        public float GetSignFrequency()
        {
            return signFrequency;
        }
        public void SetSignFrequency(float value)
        {
            signFrequency = value;
        }

        public int GetMinChestStrength()
        {
            return minChestStrength;
        }
        public void SetMinChestStrength(int value)
        {
            minChestStrength = value;
        }

        public int GetMaxChestStrength()
        {
            return maxChestStrength;
        }
        public void SetMaxChestStrength(int value)
        {
            maxChestStrength = value;
        }

        public int GetMinCaptainsCount()
        {
            return minCaptainsCount;
        }
        public void SetMinCaptainsCount(int value)
        {
            minCaptainsCount = value;
        }

        public int GetMaxCaptainsCount()
        {
            return maxCaptainsCount;
        }
        public void SetMaxCaptainsCount(int value)
        {
            maxCaptainsCount = value;
        }

        public int GetMinCaptainsGroups()
        {
            return minCaptainsGroups;
        }
        public void SetMinCaptainsGroups(int value)
        {
            minCaptainsGroups = value;
        }

        public int GetMaxCaptainsGroups()
        {
            return maxCaptainsGroups;
        }
        public void SetMaxCaptainsGroups(int value)
        {
            maxCaptainsGroups = value;
        }

        public int GetMinCaptainStrength()
        {
            return minCaptainStrength;
        }
        public void SetMinCaptainStrength(int value)
        {
            minCaptainStrength = value;
        }

        public int GetMaxCaptainStrength()
        {
            return maxCaptainStrength;
        }
        public void SetMaxCaptainStrength(int value)
        {
            maxCaptainStrength = value;
        }

        public int GetCitiesCount()
        {
            return citiesCount;
        }
        public void SetCitiesCount(int value)
        {
            citiesCount = value;
        }

        public int GetCastlesCount()
        {
            return castlesCount;
        }
        public void SetCastlesCount(int value)
        {
            castlesCount = value;
        }

        public int GetCastleStrengthBonus()
        {
            return castleStrengthBonus;
        }

        public void SetCastleStrengthBonus(int strengthBonus)
        {
            castleStrengthBonus = strengthBonus;
        }

        public int GetMinDwellingsCount()
        {
            return minDwellingsCount;
        }
        public void SetMinDwellingsCount(int value)
        {
            minDwellingsCount = value;
        }

        public int GetMaxDwellingsCount()
        {
            return maxDwellingsCount;
        }
        public void SetMaxDwellingsCount(int value)
        {
            maxDwellingsCount = value;
        }

        public int GetMinDwellingCreatureLevel()
        {
            return minDwellingCreatureLevel;
        }
        public void SetMinDwellingCreatureLevel(int value)
        {
            minDwellingCreatureLevel = value;
        }

        public int GetMaxDwellingCreatureLevel()
        {
            return maxDwellingCreatureLevel;
        }
        public void SetMaxDwellingCreatureLevel(int value)
        {
            maxDwellingCreatureLevel = value;
        }
    }
}