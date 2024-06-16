using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Shared.Logic.Character;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnChangePosition : EventArgs
    {
        public int X, Y;

        public OnChangePosition(int x, int y)
        {
            X = x;
            Y = y;

            Player.Instance().X = x;
            Player.Instance().Y = y;

            if (Player.Instance().IsNavigating())
            {
                Boat.Instance().X = x;
                Boat.Instance().Y = y;
            }

            ContinentSystem
                .Instance()
                .GetContinentAtNumber(Player.Instance().ContinentNumber)
                .ChangeTilesVisibility(Player.Instance().GetPosition(), 3, true);
        }
    }
}