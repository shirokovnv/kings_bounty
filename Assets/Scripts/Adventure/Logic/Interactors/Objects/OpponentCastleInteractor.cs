using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Adventure.UI.Background;
using Assets.Scripts.Adventure.UI.BottomPanel;
using Assets.Scripts.Combat.Logic.Systems;
using Assets.Scripts.Loading;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Data.State.Combat;
using Assets.Scripts.Shared.Logic.Character;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects
{
    public class OpponentCastleInteractor : BaseCombatableInteractor
    {
        public override void OnEntering(InteractingWithObject state)
        {
            var castle = state.Obj as Castle;

            string title = $"Castle {castle.GetName()} is under the rule of ";

            if (castle.IsContracted())
            {
                var contract = ContractSystem.Instance().GetContractByCastlePosition(
                    castle.X, castle.Y, castle.ContinentNumber
                    );

                title += $"{contract.ContractInfo().Title}";
            }
            else
            {
                title += "no one";
            }

            string text = "\r\n";
            castle.GetSquads().ForEach(group =>
            {
                text += $"{ApproximateQuantity(group.CurrentQuantity())} {group.Unit.Name} \r\n";
            });

            string laySiegeText = "Lay siege (Y/N) ?";

            BottomUIScript.Instance.ShowBottomUI();
            BottomUIScript.Instance.ShowUIMessage(title, text, null, laySiegeText);
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
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.N))
            {
                GameStateManager.Instance().SetState(
                    new InteractingWithObject
                    {
                        Stage = InteractingWithObject.InteractingStage.exiting,
                        Obj = state.Obj,
                    }
                    );
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                if (Player.Instance().HasSiegeWeapon())
                {
                    MoraleSystem.Instance().CalculateMorale((state.Obj as Castle).GetSquads());
                    PlayerSquads.Instance().CalculateModifiers();

                    GameStateManager.Instance().SetState(new Combating(
                        state.Obj as Castle,
                        PlayerSquads.Instance().GetSquadsTotalQuantity()
                        ));

                    (state.Obj as Castle).CalculateSpoilsOfWar();

                    SceneLoader.Load(SceneLoader.Scene.CombatScene);
                }
                else
                {
                    var text = "You should purchase siege weapon first...";

                    BottomUIScript.Instance.UpdateTextMessage(text);
                }
            }
        }
    }
}