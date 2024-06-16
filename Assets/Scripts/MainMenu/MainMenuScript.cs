using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Loading;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.MainMenu;
using Assets.Scripts.Shared.Logic.Character.Ranks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenu
{
    public class MainMenuScript : MonoBehaviour
    {
        private enum MainMenuState
        {
            SelectOption,
            NewGame,
            LoadGame,
        }

        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject newGameSelector;
        [SerializeField] private GameObject loadGameSelector;
        [SerializeField] private GameObject backButton;
        [SerializeField] private GameObject startButton;

        private Stack<MainMenuState> stateStack;
        private bool isStateUpdated;

        private void Awake()
        {
            stateStack = new Stack<MainMenuState>();
            stateStack.Push(MainMenuState.SelectOption);
            isStateUpdated = false;
            FillBaseSettings();

            OnStateUpdated(stateStack.Peek());
        }

        // Update is called once per frame
        void Update()
        {
            if (isStateUpdated)
            {
                OnStateUpdated(stateStack.Peek());
                backButton.SetActive(stateStack.Count > 1);
            }
        }

        public void OnNewGameClick()
        {
            stateStack.Push(MainMenuState.NewGame);
            isStateUpdated = true;
        }

        public void OnBackButtonClick()
        {
            stateStack.Pop();
            isStateUpdated = true;
        }

        public void OnStartButtonClick()
        {
            var dropDowns = newGameSelector.GetComponentsInChildren<Dropdown>();

            var difficultyLevel = Enum.Parse<TimeSystem.BaseDifficulty>(
                dropDowns[0].options[dropDowns[0].value].text);

            var rank = Enum.Parse<BaseRank>(
                dropDowns[1].options[dropDowns[1].value].text);

            var characterName = newGameSelector.GetComponentInChildren<InputField>().text;

            GameStateManager.Instance().SetState(new NewGame
            {
                DifficultyLevel = difficultyLevel,
                Rank = rank,
                CharacterName = characterName
            });

            SceneLoader.Load(SceneLoader.Scene.AdventureScene);
        }

        public void OnLoadButtonClick()
        {
            stateStack.Push(MainMenuState.LoadGame);
            isStateUpdated = true;
        }

        public void OnExitButtonClick()
        {
            Application.Quit();
        }

        public void OnChangeCharacterName(string name)
        {
            startButton.GetComponentInChildren<Button>().interactable = name != string.Empty;
        }

        private void OnStateUpdated(MainMenuState state)
        {
            switch (state)
            {
                case MainMenuState.SelectOption:
                    mainMenu.SetActive(true);
                    newGameSelector.SetActive(false);
                    loadGameSelector.SetActive(false);
                    startButton.SetActive(false);

                    break;

                case MainMenuState.NewGame:
                    mainMenu.SetActive(false);
                    newGameSelector.SetActive(true);
                    loadGameSelector.SetActive(false);
                    startButton.SetActive(true);
                    var characterName = newGameSelector.GetComponentInChildren<InputField>().text;
                    startButton.GetComponentInChildren<Button>().interactable = characterName != string.Empty;

                    break;

                case MainMenuState.LoadGame:
                    mainMenu.SetActive(false);
                    newGameSelector.SetActive(false);
                    loadGameSelector.SetActive(true);
                    startButton.SetActive(false);

                    LoadSavedGameScript.Instance.ViewAllSavedGames();

                    break;
            }

            isStateUpdated = false;
        }

        private void FillBaseSettings()
        {
            var difficultyLevels = Enum.GetNames(typeof(TimeSystem.BaseDifficulty));
            var difficultyOptions = new List<Dropdown.OptionData>(
                difficultyLevels.Select(difficulty => new Dropdown.OptionData(difficulty))
                );

            var ranks = Enum.GetNames(typeof(BaseRank));
            var rankOptions = new List<Dropdown.OptionData>(
                ranks.Select(rank => new Dropdown.OptionData(rank))
                );

            var dropDowns = newGameSelector.GetComponentsInChildren<Dropdown>();

            dropDowns[0].options = difficultyOptions;
            dropDowns[1].options = rankOptions;
        }
    }
}