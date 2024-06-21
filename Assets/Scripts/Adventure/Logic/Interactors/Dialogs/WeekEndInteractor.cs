using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Accounting;
using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Logic.Character;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Dialogs
{
    public class WeekEndInteractor : BaseDialogInteractor<ViewWeekEnd, DidViewWeekEnd>
    {
        private const int GROW_PERIOD_IN_WEEKS = 4;

        public delegate UnitScriptableObject NextActivatedUnit();
        public static NextActivatedUnit Activator;

        private readonly static List<ITaxable> taxables = new()
        {
            PlayerSquads.Instance(),
            Boat.Instance(),
        };

        public override void WaitForStart()
        {
            ViewWeekEnd state = GameStateManager.Instance().GetState() as ViewWeekEnd;             

            Activator ??= () => UnitManager.Instance().GetRandomUnitExceptType(Dwelling.DwellingType.castle);

            var activatedUnit = Activator();

            string title = "Week # " + state.WeekNumber;
            string text = string.Empty;

            text += "Astrologers proclaim: Week of the " + activatedUnit.Name + " \r\n";
            text += "All " + activatedUnit.Name + " dwellings are repopulated. \r\n";

            text += "\r\n";

            text += AccountManager.GetBalanceInfo(taxables).Info;

            DialogUI.Instance.ShowDialogUI(title, text, null);

            GameStateManager.Instance().SetState(new DidViewWeekEnd
            {
                WeekNumber = state.WeekNumber,
                AfterSpentCallback = () =>
                {
                    CheckWeekOfPeasants(activatedUnit);
                    PopulateDwellings(activatedUnit);
                    PopulateCastles();

                    if (state.WeekNumber % GROW_PERIOD_IN_WEEKS == 0)
                    {
                        GrowEnemiesInNumbers();
                    }

                    state.AfterSpentCallback?.Invoke();
                    Activator = null;
                },
            });
        }

        public override void WaitForFinish()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
            {
                var afterSpentCallback = (GameStateManager.Instance().GetState() as DidViewWeekEnd).AfterSpentCallback;

                GameStateManager.Instance().SetState(new Adventuring());

                int balance = AccountManager.GetBalanceInfo(taxables).Balance;

                if (balance < 0)
                {
                    Debug.Log("NEGATIVE BALANCE!");

                    var squads = PlayerSquads.Instance().GetSquads();
                    var mostExpensiveSquad = squads
                        .OrderByDescending(squad => squad.GetTax())
                        .FirstOrDefault();

                    if (mostExpensiveSquad != null)
                    {
                        PlayerSquads.Instance().RemoveSquad(squads.IndexOf(mostExpensiveSquad));
                    }

                    balance = 0;
                }

                PlayerStats.Instance().SetGold(balance);

                DialogUI.Instance.HideDialog();

                afterSpentCallback?.Invoke(); ;
            }
        }

        private void PopulateDwellings(UnitScriptableObject unit)
        {
            ContinentSystem
                .Instance()
                .GetContinents()
                .SelectMany(c => c.GetObjectsOfType(ObjectType.dwelling))
                .Select(d => d as Dwelling)
                .Where(d => d.Unit.GetScriptID() == unit.GetScriptID())
                .ToList()
                .ForEach(d =>
                {
                    d.SetCurrentPopulation(d.Unit.Population);
                });
        }

        private void PopulateCastles()
        {
            var emptyCastles = ContinentSystem
                    .Instance()
                    .GetContinents()
                    .SelectMany(c => c.GetCastles().Where(castle => castle.GetOwner() == CastleOwner.player))
                    .Where(castle => castle.GetSquads().Count() == 0)
                    .ToList();

            var strength = PlayerStats.Instance().GetLeadership();

            Debug.Log("POPULATE CASTLES!");

            emptyCastles.ForEach(castle =>
            {
                var strengthBonus = ContinentSystem
                    .Instance()
                    .GetContinentAtNumber(castle.ContinentNumber)
                    .GetConfig()
                    .GetObjectConfig()
                    .GetCastleStrengthBonus();

                castle.BuildGarrison(UnitManager.Instance().GetUnits(), strength + strengthBonus);
                castle.SetOwner(CastleOwner.opponent);

                PlayerStats.Instance().DecreaseCastlesGarrisoned();
            });
        }

        private void CheckWeekOfPeasants(UnitScriptableObject activatedUnit)
        {
            if (activatedUnit != null && activatedUnit.Name == "Peasants")
            {
                var ghosts = PlayerSquads
                    .Instance()
                    .GetSquads()
                    .Where(squad => squad.Unit.Name == "Ghosts")
                    .FirstOrDefault();

                if (ghosts != null)
                {
                    int ghostIndex = PlayerSquads.Instance().GetSquads().IndexOf(ghosts);
                    int quantity = ghosts.CurrentQuantity();

                    PlayerSquads.Instance().RemoveSquad(ghostIndex);
                    PlayerSquads.Instance().AddOrRefillSquad(activatedUnit, quantity);
                }
            }
        }

        private void GrowEnemiesInNumbers()
        {
            var castles = ContinentSystem
                .Instance()
                .GetContinents()
                .SelectMany(c => c.GetCastles())
                .Where(castle => castle.GetOwner() == CastleOwner.opponent)
                .ToList();

            castles.ForEach(castle => castle.GrowInNumbers());

            var captains = ContinentSystem
                .Instance()
                .GetContinents()
                .SelectMany(c => c.GetObjectsOfType(ObjectType.captain))
                .Select(obj => obj as Captain)
                .ToList();

            captains.ForEach(captain => captain.GrowInNumbers());
        }
    }
}