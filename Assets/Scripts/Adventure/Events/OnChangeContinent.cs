using Assets.Scripts.Shared.Logic.Character;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnChangeContinent : EventArgs
    {
        public int ContinentNumber;

        public OnChangeContinent(int continentNumber)
        {
            ContinentNumber = continentNumber;

            Player.Instance().ContinentNumber = continentNumber;
            Boat.Instance().ContinentNumber = continentNumber;
        }
    }
}