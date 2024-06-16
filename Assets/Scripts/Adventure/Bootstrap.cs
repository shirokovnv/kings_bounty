using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Bounties;
using Assets.Scripts.Adventure.Logic.Continents.Config;
using Assets.Scripts.Adventure.Logic.Puzzle;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Shared.Data;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Logic.Character.Ranks;
using Assets.Scripts.Shared.Logic.Systems;
using Assets.Scripts.Shared.Data.State;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Data.State.Combat;
using Assets.Scripts.Shared.Data.State.MainMenu;
using Assets.Scripts.Shared.Utility;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure
{
    [DefaultExecutionOrder(-1000)]
    public class Bootstrap : MonoBehaviour
    {
        private const string DEFAULT_CHAR_NAME = "Player";

        // Start is called before the first frame update
        void Start()
        {
            GameState state = GameStateManager.Instance().GetState();

            state ??= new NewGame
            {
                CharacterName = DEFAULT_CHAR_NAME,
                DifficultyLevel = TimeSystem.BaseDifficulty.Normal,
                Rank = Utils.RandomEnumValue<BaseRank>()
            };

            switch (state)
            {
                case NewGame:
                    OnNewGame(
                        (state as NewGame).CharacterName,
                        (state as NewGame).Rank,
                        (state as NewGame).DifficultyLevel
                        );
                    break;

                case LoadGame:
                    OnLoadGame((state as LoadGame).FileName);
                    break;

                case WinCombat:
                    OnWinCombat();
                    break;

                case LoseCombat:
                    OnLoseCombat();
                    break;
            }
        }

        private void OnNewGame(
            string charName,
            BaseRank baseRank,
            TimeSystem.BaseDifficulty difficulty
        )
        {
            NamingData.Instance().ResetUsedCastleNames();
            NamingData.Instance().ResetUsedTownNames();

            // generate new continents
            var configPack = new ContinentConfigPack(UnitManager.Instance().GetUnits());
            ContinentSystem.Instance().Generate(configPack);
            ContinentSystem.Instance().GetContinentAtNumber(1).SetRevealed(true);

            // set rank
            Rank rank = baseRank switch
            {
                BaseRank.Knight => new Knight(),
                BaseRank.Ranger => new Ranger(),
                BaseRank.Sorceress => new Sorceress(),
                _ => new Rank[] {
                new Knight(),
                new Ranger(),
                new Sorceress()
            }.OrderBy(_ => Random.value).FirstOrDefault()
            };

            // set player
            var player = Player.Instance();

            player.ContinentNumber = 1;
            player.SetRank(rank);
            player.SetName(charName);
            player.SetDirection(Player.Direction.right);
            player.SetNavigating(false);
            player.SetEndurance(1);

            Boat.Instance().X = -1;
            Boat.Instance().Y = -1;
            Boat.Instance().ContinentNumber = 1;
            Boat.Instance().IsActive = false;

            // build contracts
            ContractSystem.Instance().BuildContracts(
                ContinentSystem.Instance().GetContinents(),
                new ContractBuilder()
                );

            // Set Time
            TimeSystem.Instance().SetDifficulty(difficulty);
            TimeSystem.Instance().SetMovesPerDay(rank.GetMovesPerDay());
            TimeSystem.Instance().ResetDaysCount();

            // set initial state
            GameStateManager.Instance().SetState(new ViewGreetings());

            // change player position
            Vector2Int startPosition = ContinentSystem.Instance().GetContinentAtNumber(1).GetStartPosition();
            EventBus.Instance.PostEvent(new OnChangePosition(startPosition.x, startPosition.y));

            // reset player stats
            PlayerStats stats = PlayerStats.Instance();

            stats.SetBaseStats(Player.Instance().GetRank());

            // Create puzzle map
            var puzzleBag = new PuzzleBag(
                ContinentSystem.Instance().GetContinents(),
                ContractSystem.Instance().GetContracts());

            var puzzleLocator = new PuzzleLocator(ContinentSystem.Instance().GetContinents());

            PuzzleSystem.Instance().Create(puzzleBag, puzzleLocator);

            CaptainMovementSystem.Instance().SetMode(CaptainMovementSystem.CaptainMode.Move);

            var squads = PlayerSquads.Instance();
            squads.SetBaseSquads(Player.Instance().GetRank());

            EventBus.Instance.PostEvent(new OnChangeSpellPower { Power = PlayerStats.Instance().GetSpellPower() });
            EventBus.Instance.PostEvent(new OnNewGame());
        }

        private void OnLoadGame(string fileName)
        {
            FileSystem.Load(fileName);

            PostEventsAfterSceneChanged();

            EventBus.Instance.PostEvent(new OnLoadGame());

            GameStateManager.Instance().SetState(new Adventuring());
        }

        private void OnWinCombat()
        {
            PostEventsAfterSceneChanged();
        }

        private void OnLoseCombat()
        {
            PostEventsAfterSceneChanged();
        }

        private void PostEventsAfterSceneChanged()
        {
            var position = Player.Instance().GetPosition();

            EventBus.Instance.PostEvent(new OnChangePosition(position.x, position.y));
            EventBus.Instance.PostEvent(new OnPuzzleCreated { PuzzleInfo = PuzzleSystem.Instance().GetPuzzleInfo() });
            EventBus.Instance.PostEvent(new OnChangeSpellPower { Power = PlayerStats.Instance().GetSpellPower() });
            EventBus.Instance.PostEvent(new OnBuySiegeWeaponEvent { IsPurchased = Player.Instance().HasSiegeWeapon() });
            EventBus.Instance.PostEvent(new OnNewContract { Contract = ContractSystem.Instance().GetCurrentContract() });
        }
    }
}