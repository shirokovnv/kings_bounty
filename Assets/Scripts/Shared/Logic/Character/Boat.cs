using Assets.Scripts.Adventure.Logic.Accounting;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Character
{
    [System.Serializable]
    public class Boat : ITaxable, ISerializationCallbackReceiver
    {
        private static Boat instance;

        public int X, Y, ContinentNumber;
        public bool IsActive;
        public int Discount;

        private Boat() { }

        public static Boat Instance()
        {
            instance ??= new Boat();

            return instance;
        }

        public int GetTax()
        {
            return (IsActive || Player.Instance().IsNavigating()) ? 500 - Discount : 0;
        }

        public string GetTaxInfo()
        {
            return "Boat: " + GetTax();
        }

        public void OnAfterDeserialize()
        {
            instance = this;
        }

        public void OnBeforeSerialize()
        {
        }
    }
}