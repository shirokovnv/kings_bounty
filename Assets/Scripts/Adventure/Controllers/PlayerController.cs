using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Interactors;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Loading;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Data.State.MainMenu;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Logic.Systems;
using UnityEngine;

namespace Assets.Scripts.Adventure.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Sprite[] playerSprites;
        [SerializeField] private Sprite[] boatSprites;

        protected const float frameRatio = 0.2f;

        protected int currentFrame;
        protected float timer;

        private void Awake()
        {
            currentFrame = 0;
            EventBus.Instance.Register(this);
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;

            var sprites = Player.Instance().IsNavigating() ? boatSprites : playerSprites;

            if (timer > frameRatio)
            {
                timer -= frameRatio;
                currentFrame = (currentFrame + 1) % (sprites.Length - 1);
            }

            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[currentFrame];
            gameObject.GetComponent<SpriteRenderer>().flipX = Player.Instance().GetDirection() == Player.Direction.left;

            int continentNumber = Player.Instance().ContinentNumber;

            var interactor = new InteractionController(
                ContinentSystem.Instance().GetContinentAtNumber(continentNumber)
                );

            var state = GameStateManager.Instance().GetState();
            interactor.Interact(state);

            if (state is Adventuring)
            {
                if (Input.GetKeyDown(KeyCode.F10))
                {
                    Application.Quit();
                }

                if (Input.GetKeyDown(KeyCode.W))
                {
                    TimeSystem.Instance().WeekEnd();
                    Debug.Log("Days left: " + TimeSystem.Instance().DaysLeft());
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    FileSystem.Save();

                    TopUIScript.Instance.UpdateTextMessage("Game saved!");
                }

                if (Input.GetKeyDown(KeyCode.L))
                {
                    var path = FileSystem.SAVE_DIR + $"{Player.Instance().GetName()}.{FileSystem.SAVE_EXTENSION}";

                    if (FileSystem.Exists(path))
                    {
                        GameStateManager.Instance().SetState(new LoadGame
                        {
                            FileName = FileSystem.SAVE_DIR + $"{Player.Instance().GetName()}.{FileSystem.SAVE_EXTENSION}"
                        });

                        SceneLoader.Load(SceneLoader.Scene.AdventureScene);

                        TopUIScript.Instance.UpdateTextMessage("Game loaded!");

                        return;
                    }

                    TopUIScript.Instance.UpdateTextMessage($"File {path} does not exist.");
                }
            }
        }

        public void OnEvent(OnChangePosition e)
        {
            gameObject.transform.position = new Vector3(e.X, e.Y);

            var sprites = Player.Instance().IsNavigating() ? boatSprites : playerSprites;

            timer = 0;
            currentFrame = sprites.Length - 1;
        }

        public void OnEvent(OnFoundGrail e)
        {
            GameStateManager.Instance().SetState(new WinScenario());

            SceneLoader.Load(SceneLoader.Scene.EndingScene);
        }

        public void OnEvent(OnTimeIsOut e)
        {
            GameStateManager.Instance().SetState(new LoseScenario());

            SceneLoader.Load(SceneLoader.Scene.EndingScene);
        }
    }
}