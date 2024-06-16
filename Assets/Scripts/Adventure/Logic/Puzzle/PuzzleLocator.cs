using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Puzzle
{
    public class PuzzleLocator : IPuzzleLocator
    {
        private readonly List<Continent> continents;

        public PuzzleLocator(List<Continent> continents)
        {
            this.continents = continents;
        }

        public (Vector2Int, int) SelectCenter()
        {
            var continent = continents.ElementAt(Random.Range(0, continents.Count));
            int continentNumber = continent.GetConfig().GetNumber();

            var positions = continent.GetAllGroundPositionsExceptCastleGate();
            var center = positions.ElementAt(Random.Range(0, positions.Count));

            return (center, continentNumber);
        }
    }
}