using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Shared.Logic.Character.Ranks;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Character
{
    [System.Serializable]
    public class PlayerStats : ISerializationCallbackReceiver
    {
        private const int BASE_GOLD = 10000;

        private const int SCORE_VILLAINS_MULTIPLIER = 500;
        private const int SCORE_CASTLES_MULTIPLIER = 100;
        private const int SCORE_ARTIFACTS_MULTIPLIER = 250;

        private static PlayerStats instance;

        [SerializeField] private int leadership;
        [SerializeField] private int weekSalary;
        [SerializeField] private int gold;
        [SerializeField] private int spellPower;
        [SerializeField] private int knowledge;
        [SerializeField] private int villainsCaught;
        [SerializeField] private int artifactsFound;
        [SerializeField] private int castlesGarrisoned;
        [SerializeField] private int followersKilled;

        private PlayerStats()
        {
        }

        public static PlayerStats Instance()
        {
            instance ??= new PlayerStats();

            return instance;
        }

        public int GetLeadership()
        {
            return leadership;
        }

        public PlayerStats SetLeadership(int leadership)
        {
            this.leadership = leadership;
            return this;
        }

        public void ChangeLeadership(int amount)
        {
            leadership += amount;
        }

        public int GetWeekSalary()
        {
            return weekSalary;
        }

        public PlayerStats SetWeekSalary(int weekSalary)
        {
            this.weekSalary = weekSalary;
            return this;
        }

        public void ChangeWeekSalary(int amount)
        {
            weekSalary += amount;
        }

        public int GetGold()
        {
            return gold;
        }

        public PlayerStats SetGold(int gold) { this.gold = gold; return this; }

        public void ChangeGold(int amount)
        {
            gold += amount;
        }

        public int GetSpellPower()
        {
            return spellPower;
        }

        public PlayerStats SetSpellPower(int spellPower)
        {
            this.spellPower = spellPower;
            return this;
        }

        public void ChangeSpellPower(int amount)
        {
            spellPower += amount;
        }

        public int GetKnowledge()
        {
            return knowledge;
        }

        public PlayerStats SetKnowledge(int knowledge)
        {
            this.knowledge = knowledge;
            return this;
        }

        public void ChangeKnowledge(int amount)
        {
            knowledge += amount;
        }

        public int GetVillainsCaught()
        {
            return villainsCaught;
        }

        public void IncreaseVillainsCaught()
        {
            villainsCaught += 1;
        }

        public int GetArtifactsFound()
        {
            return artifactsFound;
        }

        public void IncreaseArtifactsFound()
        {
            artifactsFound += 1;
        }

        public int GetCastlesGarrisoned()
        {
            return castlesGarrisoned;
        }

        public void IncreaseCastlesGarrisoned()
        {
            castlesGarrisoned += 1;
        }

        public void DecreaseCastlesGarrisoned()
        {
            castlesGarrisoned -= 1;
        }

        public int GetFollowersKilled()
        {
            return followersKilled;
        }

        public void IncreaseFollowersKilled(int amount = 1)
        {
            followersKilled += amount;
        }

        public int GetScore()
        {
            int score =
                GetVillainsCaught() * SCORE_VILLAINS_MULTIPLIER +
                GetArtifactsFound() * SCORE_ARTIFACTS_MULTIPLIER +
                GetCastlesGarrisoned() * SCORE_CASTLES_MULTIPLIER -
                GetFollowersKilled();

            if (score < 0)
            {
                score = 0;
            }

            float difficultyCoef = TimeSystem.Instance().GetDifficulty() switch
            {
                TimeSystem.BaseDifficulty.Easy => 0.5f,
                TimeSystem.BaseDifficulty.Normal => 1.0f,
                TimeSystem.BaseDifficulty.Hard => 2.0f,
                _ => 0.0f
            };

            return Mathf.FloorToInt(score * difficultyCoef);
        }

        public void SetBaseStats(IRankable rankable)
        {
            SetGold(BASE_GOLD);

            castlesGarrisoned = 0;
            artifactsFound = 0;
            followersKilled = 0;
            villainsCaught = 0;

            SetLeadership(rankable.GetBaseLeadership());
            SetSpellPower(rankable.GetBaseSpellPower());
            SetKnowledge(rankable.GetBaseKnowledge());
            SetWeekSalary(rankable.GetBaseWeekCommission());
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            instance = this;
        }
    }
}