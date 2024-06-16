using System;

namespace Assets.Scripts.Shared.Data.State.Adventure
{
    public class DidViewWeekEnd : InAdventure
    {
        public int WeekNumber;
        public Action AfterSpentCallback;
    }
}