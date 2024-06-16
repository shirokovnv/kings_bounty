using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Accounting.Transactions.Base;
using Assets.Scripts.Shared.Logic.Character;

namespace Assets.Scripts.Adventure.Logic.Accounting.Transactions
{
    public class BuySpellTransaction : BaseTransaction
    {
        private SpellScriptableObject spell;

        public BuySpellTransaction(SpellScriptableObject spell)
        {
            this.spell = spell;
        }

        public enum TransactionResult
        {
            hasBeenPurchased,
            notEnoughGold,
            notEnoughKnowledge,
        }

        public override int Result()
        {
            int gold = PlayerStats.Instance().GetGold();

            if (gold < spell.Cost)
            {
                return (int)TransactionResult.notEnoughGold;
            }

            if (Spellbook.Instance().TotalNumberOfSpells() >= PlayerStats.Instance().GetKnowledge())
            {
                return (int)TransactionResult.notEnoughKnowledge;
            }

            PlayerStats.Instance().ChangeGold(-spell.Cost);
            Spellbook.Instance().AddSpell(spell);

            return (int)TransactionResult.hasBeenPurchased;
        }
    }
}