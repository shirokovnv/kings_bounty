using Assets.Scripts.Shared.Logic.Character;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Adventure.Logic.Accounting
{
    public static class AccountManager
    {
        private readonly static int GOLD_PER_GARRISONED_CASTLE = 500;

        public static BalanceInfo GetBalanceInfo(List<ITaxable> taxables)
        {
            int gold = PlayerStats.Instance().GetGold();
            int income = PlayerStats.Instance().GetWeekSalary();

            int numGarrisonedCastles = PlayerStats.Instance().GetCastlesGarrisoned();

            income += numGarrisonedCastles * GOLD_PER_GARRISONED_CASTLE;

            string info = "---------------- Budget -------------- \r\n";
            info += "On Hand: " + gold + "\r\n";
            info += "Income: " + income + "\r\n";

            info += "Outcome: \r\n";
            info += taxables.Aggregate("", (acc, taxable) => taxable.GetTaxInfo() + "\r\n" + acc);

            int outcome = taxables.Aggregate(0, (acc, taxable) => taxable.GetTax() + acc);

            info += "----------------------------------------------\r\n";

            int balance = gold + income - outcome;

            info += "Balance: " + balance;

            return new BalanceInfo
            {
                Income = income,
                Outcome = outcome,
                Balance = balance,
                Info = info
            };
        }
    }
}