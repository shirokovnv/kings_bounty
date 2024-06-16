using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Shared.Logic.Character.Ranks;

namespace Assets.Scripts.Shared.Data.State.MainMenu
{
    public class NewGame : InMainMenu
    {
        public string CharacterName;
        public TimeSystem.BaseDifficulty DifficultyLevel;
        public BaseRank Rank;
    }
}