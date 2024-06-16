using System;

namespace Assets.Scripts.Combat.Events
{
    public class OnViewSpellbook : EventArgs
    {
        public BattleField BattleField;
        public int SourceX, SourceY;
    }
}