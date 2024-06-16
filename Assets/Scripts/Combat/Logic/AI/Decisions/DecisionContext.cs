using Assets.Scripts.Combat.Logic.AI.Grid;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Combat.Logic.AI.Decisions
{
    public class DecisionContext
    {
        public int PGroupIndex;
        public List<UnitGroup> PGroups { get; private set; }
        public List<UnitGroup> OGroups { get; private set; }

        public TileGrid<CombatGridCell> Grid;

        public DecisionContext(
            int pGroupIndex,
            List<UnitGroup> pGroups,
            List<UnitGroup> oGroups,
            TileGrid<CombatGridCell> grid
            )
        {
            PGroupIndex = pGroupIndex;
            PGroups = pGroups;
            OGroups = oGroups;
            Grid = grid;
        }

        public UnitGroup PGroup { get { return PGroups[PGroupIndex]; } }
        public int GridWidth { get { return Grid.GetWidth(); } }
        public int GridHeight { get { return Grid.GetHeight(); } }

        public void Swap()
        {
            (OGroups, PGroups) = (PGroups, OGroups);
        }

        public UnitGroup NextMoveableUnit()
        {
            var countMovable = PGroups.Where(group => group.CurrentMoves > 0).Count();

            if (countMovable == 0)
            {
                return null;
            }

            if (PGroups[PGroupIndex].CurrentMoves > 0)
            {
                return PGroups[PGroupIndex];
            }

            NextIndex();

            return NextMoveableUnit();
        }

        public int NextIndex()
        {
            PGroupIndex = (PGroupIndex + 1) % PGroups.Count;

            return PGroupIndex;
        }
    }
}