using Assets.Scripts.Shared.Logic.Character;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnCancelBoat : EventArgs
    {
        public OnCancelBoat()
        {
            Boat.Instance().X = -1;
            Boat.Instance().Y = -1;
            Boat.Instance().IsActive = false;
        }

    }
}