using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Bonuses;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Systems
{
    [Serializable]
    public class TimeSystem : ISerializationCallbackReceiver
    {
        private const int DAYS_PER_WEEK = 5;

        public enum BaseDifficulty
        {
            Easy,
            Normal,
            Hard
        }

        private static TimeSystem instance;
        [SerializeField] private BaseDifficulty difficulty;
        [SerializeField] private int ticksLeft;
        [SerializeField] private int weekNumber;
        [SerializeField] private int movesPerDay;
        [SerializeField] private List<BonusPackage> bonusPackages;
        private List<TemporaryBonus> bonuses;

        public static TimeSystem Instance()
        {
            instance ??= new TimeSystem
            {
                ticksLeft = 0,
                weekNumber = 0,
                movesPerDay = 0,
                bonuses = new List<TemporaryBonus>()
            };

            return instance;
        }

        public void Tick(int amount = 1, Action afterSpentCallback = null)
        {
            if (amount == 0)
            {
                return;
            }

            ticksLeft--;

            UpdateBonuses();

            EventBus.Instance.PostEvent(new OnTick());

            if (ticksLeft <= 0)
            {
                // time is out
                Debug.Log("TIME IS OUT!");

                EventBus.Instance.PostEvent(new OnTimeIsOut());
            }

            if (ticksLeft % movesPerDay == 0)
            {
                // day ends

                Debug.Log("Day ends");
                EventBus.Instance.PostEvent(new OnDayEnd { DaysLeft = DaysLeft() });
            }

            if (ticksLeft % (movesPerDay * DAYS_PER_WEEK) == 0)
            {
                // week ends
                Debug.Log("WEEK ENDS!");
                weekNumber++;
                EventBus.Instance.PostEvent(new OnWeekEnd
                {
                    WeekNumber = weekNumber,
                    AfterSpentCallback = afterSpentCallback
                });
            }

            Tick(amount - 1, afterSpentCallback);
        }

        public void WeekEnd(Action afterSpentCallback = null)
        {
            int remains = ticksLeft % (movesPerDay * DAYS_PER_WEEK);

            if (remains == 0)
            {
                Tick(movesPerDay * DAYS_PER_WEEK, afterSpentCallback);
            }
            else
            {
                Tick(remains, afterSpentCallback);
            }
        }

        public void SpendDays(int days, Action afterSpentCallback = null)
        {
            if (days <= 0)
            {
                return;
            }

            Tick(days * movesPerDay, afterSpentCallback);
        }

        public int DaysLeft()
        {
            int remainder = ticksLeft % movesPerDay;
            int quotient = ticksLeft / movesPerDay;

            return quotient + (remainder > 0 ? 1 : 0);
        }

        public TimeSystem AddBonus(TemporaryBonus bonus, bool updateWhenAdded = true)
        {
            var bonusLookup = bonuses.Where(b => b.GetType() == bonus.GetType()).FirstOrDefault();

            if (bonusLookup != null && bonusLookup.IsStackable() == false)
            {
                while (bonusLookup.IsCommited() && !bonusLookup.IsRolledBack())
                {
                    bonusLookup.Update();
                }

                bonuses.Remove(bonusLookup);
            }

            bonuses.Add(bonus);

            while (updateWhenAdded && !bonus.IsCommited())
            {
                bonus.Update();
            }

            return this;
        }

        public void UpdateBonuses()
        {
            bonuses.ForEach(bonus => bonus.Update());

            bonuses.RemoveAll(bonus => bonus.IsExpired());
        }

        public List<TemporaryBonus> GetActiveBonuses()
        {
            return bonuses.Where(b => !b.IsExpired()).ToList();
        }

        public int GetMovesPerDay()
        {
            return movesPerDay;
        }

        public void SetMovesPerDay(int movesPerDay)
        {
            this.movesPerDay = movesPerDay;
        }

        public int GetTicksLeft()
        {
            return ticksLeft;
        }

        public void ResetDaysCount()
        {
            int countDays = difficulty switch
            {
                BaseDifficulty.Easy => 800,
                BaseDifficulty.Normal => 600,
                BaseDifficulty.Hard => 400,
                _ => 0,
            };

            ticksLeft = countDays * movesPerDay;
        }

        public void SetDifficulty(BaseDifficulty difficulty)
        {
            this.difficulty = difficulty;
        }

        public BaseDifficulty GetDifficulty()
        {
            return difficulty;
        }

        public void OnBeforeSerialize()
        {
            bonusPackages = new List<BonusPackage>(bonuses.Select(bonus => new BonusPackage(bonus)));
        }

        public void OnAfterDeserialize()
        {
            bonuses = new List<TemporaryBonus>(bonusPackages.Select(package => package.GetBonus() as TemporaryBonus));

            instance = this;
        }
    }
}