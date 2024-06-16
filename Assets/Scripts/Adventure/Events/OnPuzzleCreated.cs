using Assets.Scripts.Adventure.Logic.Continents.Interactors.Puzzle;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnPuzzleCreated : EventArgs
    {
        public PuzzleInfo PuzzleInfo;
    }
}