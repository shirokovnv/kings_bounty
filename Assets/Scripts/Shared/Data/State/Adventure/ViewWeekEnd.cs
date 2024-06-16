using System;

namespace Assets.Scripts.Shared.Data.State.Adventure
{
    public class ViewWeekEnd : InAdventure
    {
        public int WeekNumber;
        public Action AfterSpentCallback;
    }
}