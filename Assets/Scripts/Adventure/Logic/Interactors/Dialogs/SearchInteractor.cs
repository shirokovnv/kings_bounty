using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Dialogs
{
    public class SearchInteractor : BaseDialogInteractor<Adventuring, ViewSearchInfo>
    {
        private const int DAYS_TO_SEARCH = 5;

        private enum SearchState
        {
            Default,
            Fail,
            Success
        }

        private static SearchState searchState;

        public override void WaitForStart()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !Player.Instance().IsNavigating())
            {
                GameStateManager.Instance().SetState(new ViewSearchInfo());

                searchState = SearchState.Default;

                string title = "Search:";
                string text = string.Empty;
                text += $"It will take {DAYS_TO_SEARCH} days to search in this area... \r\n";
                text += "Are you sure (Y/N) ?";

                DialogUI.Instance.ShowDialogUI(title, text, null);
            }
        }

        public override void WaitForFinish()
        {
            switch (searchState)
            {
                case SearchState.Default:
                    ProcessSearchPreparationInput();
                    break;

                case SearchState.Fail:
                    ProcessSearchResultInput();
                    break;

                case SearchState.Success:
                    ProcessSearchResultInput();
                    break;
            }
        }

        private void ProcessSearchPreparationInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.N))
            {
                GameStateManager.Instance().SetState(new Adventuring());

                DialogUI.Instance.HideDialog();
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                var player = Player.Instance();
                var puzzleInfo = PuzzleSystem.Instance().GetPuzzleInfo();
                var grailPosition = new Vector3Int(
                    puzzleInfo.CenterX,
                    puzzleInfo.CenterY,
                    puzzleInfo.ContinentNumber
                    );
                var playerPosition = new Vector3Int(
                    player.X,
                    player.Y,
                    player.ContinentNumber
                    );

                string text;

                if (playerPosition.Equals(grailPosition))
                {
                    searchState = SearchState.Success;
                    text = "Congratulations! You have found the Lost Grail!";
                }
                else
                {
                    searchState = SearchState.Fail;
                    text = "Nothing found...";
                }

                DialogUI.Instance.UpdateTextMessage(text);
            }
        }

        private void ProcessSearchResultInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance().SetState(new Adventuring());

                DialogUI.Instance.HideDialog();

                System.Action callback = searchState == SearchState.Success
                    ? OnSuccessSearchCallback
                    : null;

                TimeSystem.Instance().SpendDays(DAYS_TO_SEARCH, callback);

                searchState = SearchState.Default;
            }
        }

        private static void OnSuccessSearchCallback()
        {
            EventBus.Instance.PostEvent(new OnFoundGrail());

            Debug.Log("WIN!");
        }
    }
}