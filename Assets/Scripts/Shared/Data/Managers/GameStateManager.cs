using Assets.Scripts.Shared.Data.State;

namespace Assets.Scripts.Shared.Data.Managers
{
    public class GameStateManager
    {
        private static GameStateManager instance;

        private GameState state;

        private GameStateManager()
        {
        }

        public static GameStateManager Instance()
        {
            instance ??= new GameStateManager();

            return instance;
        }

        public void SetState(GameState state)
        {
            this.state = state;
        }

        public GameState GetState()
        {
            return state;
        }
    }
}