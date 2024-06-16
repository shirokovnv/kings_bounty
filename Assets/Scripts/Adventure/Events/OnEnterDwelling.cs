using Assets.Scripts.Adventure.Logic.Continents.Object;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnEnterDwelling : EventArgs
    {
        public Dwelling Dwelling;
        public Action<int> PurchaseCallback;
    }
}