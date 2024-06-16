using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnMoveCaptain : EventArgs
    {
        public int X, Y;
        public int TargetX, TargetY;
        public int ContinentNumber;
    }
}