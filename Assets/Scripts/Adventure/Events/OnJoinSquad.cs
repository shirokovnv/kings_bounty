using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnJoinSquad : EventArgs
    {
        public int X, Y, ContinentNumber;
    }
}