using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Utility;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Interactors.Dialogs
{
    public class ContractInteractor : BaseDialogInteractor<Adventuring, ViewContract>
    {
        public override void WaitForStart()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                var currentContract = ContractSystem.Instance().GetCurrentContract();

                var (title, body, sprite) = (default(string), default(string), null as Sprite);

                if (currentContract != null)
                {
                    title = currentContract.ContractInfo().Title;

                    body += $"Alias: {currentContract.ContractInfo().Alias} \r\n";
                    body += $"Reward: {currentContract.Reward()} \r\n";
                    body += $"Last seen: {currentContract.LastSeen()} \r\n";
                    body += $"Castle: {currentContract.Castle()} \r\n";

                    body += "Distinguishing features: \r\n";
                    body += Utils.Split(
                        currentContract.ContractInfo().DistinguishingFeatures,
                        50,
                        " ",
                        "\r\n\t"
                        ) + "\r\n";

                    body += "Crimes: \r\n";
                    body += Utils.Split(
                        currentContract.ContractInfo().Crimes,
                        50,
                        " ",
                        "\r\n\t") + "\r\n";

                    sprite = currentContract.ContractInfo().Sprites[0];
                }
                else
                {
                    title = "You have no contracts yet.";
                }

                DialogUI.Instance.ShowDialogUI(title, body, sprite);

                GameStateManager.Instance().SetState(new ViewContract());
            }
        }

        public override void WaitForFinish()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance().SetState(new Adventuring());

                DialogUI.Instance.HideDialog();
            }
        }
    }
}