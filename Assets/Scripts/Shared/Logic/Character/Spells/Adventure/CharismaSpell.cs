using Assets.Scripts.Adventure.Logic.Continents.Interactors.Systems;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Logic.Bonuses.Temporary;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Adventure
{
    public class CharismaSpell : ICastable
    {
        private const int LEADERSHIP_MULTIPLIER = 50;
        private const int DURATION_MULTIPLIER = 10;

        public void Cast()
        {
            var duration = PlayerStats.Instance().GetSpellPower() * DURATION_MULTIPLIER;
            var bonus = PlayerStats.Instance().GetSpellPower() * LEADERSHIP_MULTIPLIER;

            TimeSystem.Instance().AddBonus(new CharismaBonus(duration, bonus, false));

            DialogUI.Instance.UpdateTextMessage("Your leadership abilities temporary improved.");
        }
    }
}