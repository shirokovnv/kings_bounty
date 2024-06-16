using Assets.Scripts.Adventure.Logic.Bounties;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnNewContract : EventArgs
    {
        public Contract Contract;
    }
}