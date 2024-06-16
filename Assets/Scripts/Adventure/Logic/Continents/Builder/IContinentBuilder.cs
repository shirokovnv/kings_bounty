using Assets.Scripts.Adventure.Logic.Continents.Object.Biome;
using Assets.Scripts.Shared.Grid;

namespace Assets.Scripts.Adventure.Logic.Continents.Builder
{
    public interface IContinentBuilder
    {
        public IContinentBuilder WithInitialState();
        public IContinentBuilder WithBase(BiomeType biomeType);
        public IContinentBuilder WithWaterEdges();
        public IContinentBuilder WithSandAreas();
        public IContinentBuilder WithRiversAndLakes();
        public IContinentBuilder WithIslands(int numIslands, int minRadius, int maxRadius);
        public IContinentBuilder WithMainCastleAndCity();
        public IContinentBuilder WithCastles();
        public IContinentBuilder WithCities();
        public IContinentBuilder WithChests();
        public IContinentBuilder WithDwellings();
        public IContinentBuilder WithArtifacts();
        public IContinentBuilder WithCaptains();
        public IContinentBuilder WithSigns();
        public IContinentBuilder WithBiomes();

        TileGrid<ContinentTile> Build();
    }
}