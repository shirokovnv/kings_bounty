using Assets.Scripts.Adventure.Logic.Accounting.Transactions.Base;
using Assets.Scripts.Shared.Logic.Character;

namespace Assets.Scripts.Adventure.Logic.Accounting.Transactions
{
    public class BuySiegeWeaponTransaction : BaseTransaction
    {
        private const int SIEGE_WEAPON_COST = 3000;

        public enum TransactionResult
        {
            hasBeenPurchased,
            notEnoughGold,
            alreadyPurchased,
        }

        public override int Result()
        {
            if (Player.Instance().HasSiegeWeapon())
            {
                return (int)TransactionResult.alreadyPurchased;
            }

            int gold = PlayerStats.Instance().GetGold();
            if (gold < SIEGE_WEAPON_COST)
            {
                return (int)TransactionResult.notEnoughGold;
            }

            Player.Instance().SetHasSiegeWeapon(true);
            PlayerStats.Instance().ChangeGold(-SIEGE_WEAPON_COST);

            return (int)TransactionResult.hasBeenPurchased;
        }
    }
}