using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Puzzle
{
    public interface IPuzzleLocator
    {
        public (Vector2Int, int) SelectCenter();
    }
}