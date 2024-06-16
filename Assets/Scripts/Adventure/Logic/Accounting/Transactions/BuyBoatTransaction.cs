using Assets.Scripts.Adventure.Logic.Accounting.Transactions.Base;
using Assets.Scripts.Shared.Logic.Character;

namespace Assets.Scripts.Adventure.Logic.Accounting.Transactions
{
    public class BuyBoatTransaction : BaseTransaction
    {
        private const int BOAT_COST = 500;

        public enum TransactionResult
        {
            hasBeenPurchased,
            notEnoughGold,
            alreadyHaveBoat,
        }

        public override int Result()
        {
            if (Boat.Instance().IsActive)
            {
                return (int)TransactionResult.alreadyHaveBoat;
            }

            int gold = PlayerStats.Instance().GetGold();
            if (gold < BOAT_COST)
            {
                return (int)TransactionResult.notEnoughGold;
            }

            PlayerStats.Instance().ChangeGold(-BOAT_COST);
            Boat.Instance().IsActive = true;

            return (int)TransactionResult.hasBeenPurchased;
        }
    }
}