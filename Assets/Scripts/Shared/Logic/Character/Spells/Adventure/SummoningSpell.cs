using Assets.Scripts.Adventure.Logic.Accounting.Transactions;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Combat;
using Assets.Scripts.Shared.Data.Managers;
using System.Linq;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Adventure
{
    public class SummoningSpell : ICastable
    {
        private const int LEADERSHIP_MULTIPLIER = 20;

        public void Cast()
        {
            var spellPower = PlayerStats.Instance().GetSpellPower();
            var leadership = spellPower * LEADERSHIP_MULTIPLIER;
            var units = UnitManager.Instance().GetUnits().Where(unit => unit.HP <= leadership).ToList();

            if (units.Count == 0)
            {
                DialogUI.Instance.UpdateTextMessage("Your spellpower is too low.");

                return;
            }

            var summonedUnit = units.ElementAt(spellPower % units.Count - 1);
            var quantity = leadership / summonedUnit.HP;
            var transaction = new JoinSquadTransaction(
                new UnitGroup(summonedUnit, quantity, UnitGroup.UnitOwner.player, 0, 0)
                );

            if (transaction.Result() == (int)JoinSquadTransaction.TransactionResult.Success)
            {
                DialogUI.Instance.UpdateTextMessage($"{quantity} {summonedUnit.Name} joined your army.");

                return;
            }

            DialogUI.Instance.UpdateTextMessage($"{summonedUnit.Name} failed to join your army.");
        }
    }
}