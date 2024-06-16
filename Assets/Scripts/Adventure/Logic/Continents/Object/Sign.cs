using Assets.Scripts.Adventure.Logic.Continents.Base;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Object
{
    [System.Serializable]
    public class Sign : BaseObject
    {
        [SerializeField] private string title;

        private Sign(string name) : base(name, ObjectType.sign)
        {
        }

        public static Sign Create(int x, int y, int continentNumber)
        {
            var sign = new Sign($"sign({x}#{y}#{continentNumber})")
            {
                X = x,
                Y = y,
                ContinentNumber = continentNumber
            };

            return sign;
        }

        public string GetTitle()
        {
            return title;
        }

        public void SetTitle(string title)
        {
            this.title = title;
        }
    }
}