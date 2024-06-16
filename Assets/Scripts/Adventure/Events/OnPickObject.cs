using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnPickObject : EventArgs
    {
        public int X, Y, ContinentNumber;
    }
}