using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.UI.Background;
using Assets.Scripts.Adventure.UI.BottomPanel;
using Assets.Scripts.Combat.Interfaces;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Logic.Character;
using System;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects
{
    public class PlayerCastleInteractor : BaseObjectInteractor
    {
        private readonly KeyCode[] keyCodes = new KeyCode[]
        {
            KeyCode.A,
            KeyCode.B,
            KeyCode.C,
            KeyCode.D,
            KeyCode.E,
        };

        private enum CastleState
        {
            Army,
            Garrison
        }

        private static CastleState castleState;

        public override void OnEntering(InteractingWithObject state)
        {
            castleState = CastleState.Army;

            var castle = state.Obj as Castle;
            var fullTitle = Player.Instance().GetFullTitle();

            string title = $"Castle {castle.GetName()} is under rule of {fullTitle} \r\n";

            BottomUIScript.Instance.ShowBottomUI();
            BottomUIScript.Instance.ShowUIMessage(title, "");
            BackgroundUIScript.Instance.ShowObjectBackground(castle);

            GameStateManager.Instance().SetState(
                new InteractingWithObject
                {
                    Stage = InteractingWithObject.InteractingStage.visiting,
                    Obj = state.Obj,
                }
                );
        }

        public override void OnExiting(InteractingWithObject state)
        {
            BottomUIScript.Instance.HideBottomUI();
            BackgroundUIScript.Instance.HideBackground();

            GameStateManager.Instance().SetState(
                new Adventuring()
                );
        }

        public override void OnVisiting(InteractingWithObject state)
        {
            var switchToState = castleState.Equals(CastleState.Army)
                ? CastleState.Garrison
                : CastleState.Army;

            ISquadable from = castleState.Equals(CastleState.Army)
                ? PlayerSquads.Instance()
                : state.Obj as ISquadable;

            ISquadable to = castleState.Equals(CastleState.Army)
                ? state.Obj as ISquadable
                : PlayerSquads.Instance();

            ProcessGarrisonInput(from, to, switchToState);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance().SetState(
                    new InteractingWithObject
                    {
                        Stage = InteractingWithObject.InteractingStage.exiting,
                        Obj = state.Obj,
                    }
                    );
            }
        }

        private void ProcessGarrisonInput(ISquadable from, ISquadable to, CastleState switchTostate, bool showTax = false)
        {
            var squads = from.GetSquads();
            var text = string.Empty;

            for (int i = 0; i < squads.Count; i++)
            {
                text += char.ToUpper(Convert.ToChar(keyCodes[i])) + ") "
                    + squads[i].CurrentQuantity() + " " + squads[i].Unit.Name + "\r\n";

                if (Input.GetKeyDown(keyCodes[i]) && to.CanHireSquad(squads[i].Unit))
                {
                    var squad = squads[i];

                    from.RemoveSquad(i);
                    to.AddOrRefillSquad(squad.Unit, squad.CurrentQuantity(), squad.Owner);
                }
            }

            text += "SPACE) Switch to " + switchTostate.ToString();

            BottomUIScript.Instance.UpdateTextMessage(text);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                castleState = switchTostate;
            }
        }
    }
}