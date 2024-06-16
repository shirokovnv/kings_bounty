using Assets.Scripts.Adventure.Logic.Accounting.Transactions.Base;
using Assets.Scripts.Shared.Logic.Character;

namespace Assets.Scripts.Adventure.Logic.Accounting.Transactions
{
    public class CancelBoatTransaction : BaseTransaction
    {
        public enum TransactionResult
        {
            hasBeenCanceled,
            dontHaveBoat,
            needToGoAshore
        }

        public override int Result()
        {
            if (Player.Instance().IsNavigating())
            {
                return (int)TransactionResult.needToGoAshore;
            }

            if (!Boat.Instance().IsActive)
            {
                return (int)TransactionResult.dontHaveBoat;
            }

            Boat.Instance().IsActive = false;
            return (int)TransactionResult.hasBeenCanceled;
        }
    }
}