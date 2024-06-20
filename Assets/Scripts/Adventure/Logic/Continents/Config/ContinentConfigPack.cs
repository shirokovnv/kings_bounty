using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Continents.Builder;
using Assets.Scripts.Adventure.Logic.Continents.Object.Biome;
using Assets.Scripts.Adventure.Logic.Continents.Spec;
using System.Collections.Generic;

namespace Assets.Scripts.Adventure.Logic.Continents.Config
{
    public sealed class ContinentConfigPack
    {
        public List<ConfigPackage> Configs { get; private set; }
        public ISpecification<Continent> Specification { get; private set; }

        public ContinentConfigPack(List<UnitScriptableObject> availableUnits)
        {
            IContinentDirector director = new ContinentDirector();
            IContinentDirector islandDirector = new IslandContinentDirector();

            Configs = new();
            Specification = new ContinentSpecification();

            // 1
            var config = new ContinentConfig();
            config.SetNumber(1);

            config.GetBiomeConfig().SetMainBiome(new KeyValuePair<BiomeType, float>(BiomeType.forest, 0.5f));
            config.GetBiomeConfig().SetSubBiome(new KeyValuePair<BiomeType, float>(BiomeType.mountain, 0.5f));
            config.GetBiomeConfig().SetWaterLength(100);
            config.GetBiomeConfig().SetWaterContinuity(0.3f);

            config.GetObjectConfig().SetMinDwellingCreatureLevel(1);
            config.GetObjectConfig().SetMaxDwellingCreatureLevel(2);

            config.GetObjectConfig().SetMinChestStrength(100);
            config.GetObjectConfig().SetMaxChestStrength(500);

            config.GetObjectConfig().SetMinCaptainStrength(25);
            config.GetObjectConfig().SetMaxCaptainStrength(150);

            config.GetObjectConfig().SetCastleStrengthBonus(50);

            var builder = new ContinentBuilder(config, availableUnits);

            Configs.Add(new ConfigPackage
            {
                Name = "Continentia",
                Config = config,
                Builder = builder,
                Director = director,
            });

            // 2
            config = new ContinentConfig();
            config.SetNumber(2);

            config.GetBiomeConfig().SetMainBiome(new KeyValuePair<BiomeType, float>(BiomeType.mountain, 0.25f));
            config.GetBiomeConfig().SetSubBiome(new KeyValuePair<BiomeType, float>(BiomeType.forest, 0.75f));
            config.GetBiomeConfig().SetWaterLength(50);
            config.GetBiomeConfig().SetSandLength(50);

            config.GetObjectConfig().SetMinDwellingCreatureLevel(2);
            config.GetObjectConfig().SetMaxDwellingCreatureLevel(3);

            config.GetObjectConfig().SetMinChestStrength(250);
            config.GetObjectConfig().SetMaxChestStrength(1000);

            config.GetObjectConfig().SetMinCaptainStrength(100);
            config.GetObjectConfig().SetMaxCaptainStrength(500);

            config.GetObjectConfig().SetCastleStrengthBonus(100);

            builder = new ContinentBuilder(config, availableUnits);

            Configs.Add(new ConfigPackage
            {
                Name = "Forestria",
                Config = config,
                Builder = builder,
                Director = director,
            });


            // 3
            config = new ContinentConfig();
            config.SetNumber(3);

            config.GetBiomeConfig().SetMainBiome(new KeyValuePair<BiomeType, float>(BiomeType.forest, 0.75f));
            config.GetBiomeConfig().SetSubBiome(new KeyValuePair<BiomeType, float>(BiomeType.forest, 0.25f));
            config.GetBiomeConfig().SetWaterLength(1000);
            config.GetBiomeConfig().SetWaterContinuity(0.1f);

            config.GetBiomeConfig().SetSandLength(0);

            config.GetObjectConfig().SetMinDwellingCreatureLevel(3);
            config.GetObjectConfig().SetMaxDwellingCreatureLevel(4);

            config.GetObjectConfig().SetMinChestStrength(500);
            config.GetObjectConfig().SetMaxChestStrength(2000);

            config.GetObjectConfig().SetMinCaptainStrength(250);
            config.GetObjectConfig().SetMaxCaptainStrength(1000);

            config.GetObjectConfig().SetCastleStrengthBonus(250);

            builder = new ContinentBuilder(config, availableUnits);

            Configs.Add(new ConfigPackage
            {
                Name = "Islandia",
                Config = config,
                Builder = builder,
                Director = islandDirector,
            });


            // 4
            //config = new ContinentConfig();
            //config.SetNumber(4);

            //config.GetBiomeConfig().SetMainBiome(new KeyValuePair<BiomeType, float>(BiomeType.forest, 0.25f));
            //config.GetBiomeConfig().SetSubBiome(new KeyValuePair<BiomeType, float>(BiomeType.forest, 0.75f));
            //config.GetBiomeConfig().SetWaterLength(100);
            //config.GetBiomeConfig().SetWaterContinuity(0.7f);

            //config.GetBiomeConfig().SetSandLength(0);

            //config.GetObjectConfig().SetMinDwellingCreatureLevel(4);
            //config.GetObjectConfig().SetMaxDwellingCreatureLevel(5);

            //config.GetObjectConfig().SetMinChestStrength(1000);
            //config.GetObjectConfig().SetMaxChestStrength(2500);

            //config.GetObjectConfig().SetMinCaptainStrength(1000);
            //config.GetObjectConfig().SetMaxCaptainStrength(2500);

            //config.GetObjectConfig().SetCastleStrengthBonus(500);

            //builder = new ContinentBuilder(config, availableUnits);

            //Configs.Add(new ConfigPackage
            //{
            //    Name = "Alpia",
            //    Config = config,
            //    Builder = builder,
            //    Director = director,
            //});


            // 5
            config = new ContinentConfig();
            config.SetNumber(4);

            config.GetBiomeConfig().SetMainBiome(new KeyValuePair<BiomeType, float>(BiomeType.mountain, 0.75f));
            config.GetBiomeConfig().SetSubBiome(new KeyValuePair<BiomeType, float>(BiomeType.mountain, 0.25f));
            config.GetBiomeConfig().SetWaterLength(20);
            config.GetBiomeConfig().SetWaterContinuity(0.9f);
            config.GetBiomeConfig().SetSandLength(0);

            config.SetSandRingEdge(7);

            config.GetObjectConfig().SetMinDwellingCreatureLevel(5);
            config.GetObjectConfig().SetMaxDwellingCreatureLevel(6);

            config.GetObjectConfig().SetMinChestStrength(1000);
            config.GetObjectConfig().SetMaxChestStrength(2500);

            config.GetObjectConfig().SetMinCaptainStrength(1000);
            config.GetObjectConfig().SetMaxCaptainStrength(2500);

            config.GetObjectConfig().SetCastleStrengthBonus(500);

            builder = new ContinentBuilder(config, availableUnits);

            Configs.Add(new ConfigPackage
            {
                Name = "Saharia",
                Config = config,
                Builder = builder,
                Director = director,
            });

        }
    }
}