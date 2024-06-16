using Assets.Scripts.Adventure.Logic.Bounties;
using Assets.Scripts.Adventure.Logic.Continents.Interactors.Systems;
using Assets.Scripts.Adventure.UI.Dialog;
using System.Linq;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Adventure
{
    public class RevealContractSpell : ICastable
    {
        public void Cast()
        {
            Contract contract = ContractSystem.Instance().GetCurrentContract();

            if (contract != null)
            {
                var (position, continentNumber) = (contract.Position(), contract.ContinentNumber());

                var continent = ContinentSystem.Instance().GetContinentAtNumber(continentNumber);
                var castle = continent.GetCastles()
                    .Where(castle => castle.X == position.x && castle.Y == position.y)
                    .FirstOrDefault();

                contract.SetLastSeen(continent.GetName());
                contract.SetCastle(castle.GetName());

                DialogUI.Instance.UpdateTextMessage(
                    $"You are looking for castle {castle.GetName()} on {continent.GetName()} : X = {castle.X}, Y = {castle.Y}"
                    );

                return;
            }

            DialogUI.Instance.UpdateTextMessage("You have no active contracts.");
        }
    }
}