using Assets.Scripts.Shared.Logic.Character;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnShore : EventArgs
    {
        public int X, Y;

        public OnShore(int x, int y)
        {
            X = x;
            Y = y;

            Player.Instance().X = x;
            Player.Instance().Y = y;

            Player.Instance().SetNavigating(false);
            Boat.Instance().IsActive = true;
        }
    }
}