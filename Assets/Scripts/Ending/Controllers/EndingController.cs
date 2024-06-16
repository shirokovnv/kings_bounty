using Assets.Scripts.Loading;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Logic.Character;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ending.Controllers
{
    public class EndingController : MonoBehaviour
    {
        [SerializeField] private Sprite[] endingSprites;
        [SerializeField] private GameObject textObject;

        private static readonly string[] WIN_SCENARIO_TEXT =
        {
        "<size=32><b>Congratulations!</b></size>",
        "",
        "You have recovered <color=#ffff55>The Lost Grail</color> from the clutches of the kingdom conspirators.",
        "",
        "As a reward for saving himself and archipelago from ruin,",
        "King Maximus and his subjects reward you with a large",
        "parcel of land, a rank of nobility and a medal, announcing",
        $"your <color=#ffff55>Final Score ({PlayerStats.Instance().GetScore()})</color>."
    };

        private static readonly string[] LOSE_SCENARIO_TEXT =
        {
        "<size=32><b>Oh,</b></size>",
        "",
        "you have failed to recover <color=#ffff55>The Lost Grail</color> in time to save the land!",
        "",
        "Beloved King Maximus has died and the palace conspirators rules his place.",
        "",
        "The archipelago lay in ruin about you, and its people",
        "doomed to a life of misery and oppression..."
    };

        private void Awake()
        {
            var state = GameStateManager.Instance().GetState();
            GetComponentInChildren<Image>().sprite = BuildFinalImage(state);

            textObject.GetComponent<Text>().text = BuildFinalText(state);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance().SetState(null);

                SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
            }
        }

        private string BuildFinalText(GameState state)
        {
            if (state is WinScenario)
            {
                return string.Join("\r\n", WIN_SCENARIO_TEXT);
            }

            if (state is LoseScenario)
            {
                return string.Join("\r\n", LOSE_SCENARIO_TEXT);
            }

            return string.Empty;
        }

        private Sprite BuildFinalImage(GameState state)
        {
            return state switch
            {
                WinScenario => endingSprites[0],
                LoseScenario => endingSprites[1],
                _ => null
            };
        }
    }
}