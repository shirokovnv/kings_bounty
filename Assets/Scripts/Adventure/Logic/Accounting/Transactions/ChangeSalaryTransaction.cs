using Assets.Scripts.Adventure.Logic.Accounting.Transactions.Base;
using Assets.Scripts.Shared.Logic.Character;

namespace Assets.Scripts.Adventure.Logic.Accounting.Transactions
{
    public class ChangeSalaryTransaction : BaseTransaction
    {
        private readonly int amount;

        public ChangeSalaryTransaction(int amount)
        {
            this.amount = amount;
        }

        public override int Result()
        {
            PlayerStats.Instance().ChangeWeekSalary(amount);

            return 0;
        }
    }
}