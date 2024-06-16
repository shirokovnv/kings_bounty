using Assets.Scripts.Shared.Data.State;
using System;

namespace Assets.Scripts.Adventure.Events
{
    public class OnWeekEnd : InAdventure
    {
        public int WeekNumber;
        public Action AfterSpentCallback;
    }
}