using Assets.Scripts.Shared.Logic.Character;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnBuyBoat : EventArgs
    {
        public int X, Y, ContinentNumber;

        public OnBuyBoat(int x, int y, int continentNumber)
        {
            X = x;
            Y = y;
            ContinentNumber = continentNumber;

            Boat.Instance().X = X;
            Boat.Instance().Y = Y;
            Boat.Instance().ContinentNumber = ContinentNumber;
            Boat.Instance().IsActive = true;
        }
    }
}