using Assets.Scripts.Shared.Grid;

namespace Assets.Scripts.Adventure.Logic.Continents.Builder
{
    public interface IContinentDirector
    {
        public TileGrid<ContinentTile> Make(IContinentBuilder builder);
    }
}