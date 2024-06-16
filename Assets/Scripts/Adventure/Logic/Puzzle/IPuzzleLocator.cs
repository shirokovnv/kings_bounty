using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Puzzle
{
    public interface IPuzzleLocator
    {
        public (Vector2Int, int) SelectCenter();
    }
}