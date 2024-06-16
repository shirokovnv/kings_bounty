using System.Collections.Generic;

namespace Assets.Scripts.Adventure.Logic.Puzzle
{
    public interface IPuzzleBag
    {
        public List<PuzzlePiece> GetPuzzles();
    }
}