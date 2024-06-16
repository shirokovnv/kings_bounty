using Assets.Scripts.Adventure.Logic.Continents;
using Assets.Scripts.Adventure.Logic.Continents.Object.Biome;
using Assets.Scripts.Shared.Grid;

namespace Assets.Scripts.Adventure.Logic.Continents.Builder
{
    public class ContinentDirector : IContinentDirector
    {
        public TileGrid<ContinentTile> Make(IContinentBuilder builder)
        {
            return builder
                .WithInitialState()
                .WithWaterEdges()
                .WithBase(BiomeType.grass)
                .WithRiversAndLakes()
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