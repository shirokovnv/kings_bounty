using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Accounting.Transactions.Base;
using Assets.Scripts.Shared.Logic.Character;

namespace Assets.Scripts.Adventure.Logic.Accounting.Transactions
{
    public class BuySquadTransaction : BaseTransaction
    {
        public enum TransactionResult
        {
            hasBeenPurchased,
            notEnoughGold,
            notEnoughPopulation,
            notEnoughLeadership,
            cantBuyZeroAmount,
        }

        private readonly UnitScriptableObject unit;
        private readonly int amount;
        private readonly int maxAmount;
        private readonly int population;

        public BuySquadTransaction(UnitScriptableObject unit, int amount, int maxAmount, int population)
        {
            this.unit = unit;
            this.amount = amount;
            this.maxAmount = maxAmount;
            this.population = population;
        }

        public override int Result()
        {
            if (amount == 0)
            {
                return (int)TransactionResult.cantBuyZeroAmount;
            }

            int gold = PlayerStats.Instance().GetGold();

            if (gold < unit.Cost * amount)
            {
                return (int)TransactionResult.notEnoughGold;
            }

            if (amount > population)
            {
                return (int)TransactionResult.notEnoughPopulation;
            }

            if (amount > maxAmount)
            {
                return (int)TransactionResult.notEnoughLeadership;
            }

            PlayerStats.Instance().ChangeGold(-unit.Cost * amount);
            PlayerSquads.Instance().AddOrRefillSquad(unit, amount);

            return (int)TransactionResult.hasBeenPurchased;
        }
    }
}