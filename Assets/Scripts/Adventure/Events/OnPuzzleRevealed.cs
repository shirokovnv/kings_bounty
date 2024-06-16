using Assets.Scripts.Adventure.Logic.Continents.Interactors.Puzzle;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnPuzzleRevealed : EventArgs
    {
        public PuzzlePiece PuzzlePiece;
        public int X, Y;
    }
}