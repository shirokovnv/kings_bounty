using Assets.Scripts.Adventure.Logic.Puzzle;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnPuzzleCreated : EventArgs
    {
        public PuzzleInfo PuzzleInfo;
    }
}