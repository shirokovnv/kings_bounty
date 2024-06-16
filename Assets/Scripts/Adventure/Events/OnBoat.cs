using Assets.Scripts.Shared.Logic.Character;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnBoat : EventArgs
    {
        public int X, Y;

        public OnBoat(int x, int y)
        {
            X = x;
            Y = y;

            Player.Instance().X = x;
            Player.Instance().Y = y;

            Player.Instance().SetNavigating(true);
            Boat.Instance().IsActive = false;
        }
    }
}