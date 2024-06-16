using Assets.Scripts.Adventure.Logic.Accounting.Transactions.Base;
using Assets.Scripts.Combat;
using Assets.Scripts.Shared.Logic.Character;

namespace Assets.Scripts.Adventure.Logic.Accounting.Transactions
{
    public class JoinSquadTransaction : BaseTransaction
    {
        public enum TransactionResult
        {
            Success,
            Failure
        }

        private readonly UnitGroup squad;

        public JoinSquadTransaction(UnitGroup squad)
        {
            this.squad = squad;
        }

        public override int Result()
        {
            if (PlayerSquads.Instance().CanHireSquad(squad.Unit))
            {
                PlayerSquads.Instance().AddOrRefillSquad(squad.Unit, squad.CurrentQuantity());

                return (int)TransactionResult.Success;
            }

            return (int)TransactionResult.Failure;
        }
    }
}