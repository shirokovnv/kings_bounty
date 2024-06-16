using Assets.Scripts.Adventure.Logic.Continents.Object.Biome;
using Assets.Scripts.Shared.Grid;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Builder
{
    public class IslandContinentDirector : IContinentDirector
    {
        public TileGrid<ContinentTile> Make(IContinentBuilder builder)
        {
            int numIslands = Random.Range(25, 50);

            return builder
                .WithInitialState()
                .WithBase(BiomeType.water)
                .WithIslands(numIslands, 8, 16)
                .WithWaterEdges()
                .WithSandAreas()
                .WithMainCastleAndCity()
                .WithCastles()
                .WithBiomes()
                .WithCities()
                .WithChests()
                .WithSigns()
                .WithArtifacts()
                .WithCaptains()
                .WithDwellings()
                .Build();
        }
    }
}