using Assets.Scripts.Adventure.Logic.Accounting.Transactions.Base;
using Assets.Scripts.Shared.Logic.Character;

namespace Assets.Scripts.Adventure.Logic.Accounting.Transactions.Factory
{
    public static class BoatTransactionFactory
    {
        public static BaseTransaction Create()
        {
            if (Boat.Instance().IsActive || Player.Instance().IsNavigating())
            {
                return new CancelBoatTransaction();
            }

            return new BuyBoatTransaction();
        }
    }
}