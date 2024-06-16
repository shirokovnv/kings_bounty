using Assets.Scripts.Combat.Interfaces;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnWinCombat : EventArgs
    {
        public ICombatable Combatable;
    }
}