using Assets.Scripts.Adventure.Logic.Continents.Interactors.Systems;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Shared.Logic.Bonuses.Temporary;

namespace Assets.Scripts.Shared.Logic.Character.Spells.Adventure
{
    public class StandStillSpell : ICastable
    {
        private const int DURATION_MULTIPLIER = 10;

        public void Cast()
        {
            TimeSystem.Instance().AddBonus(
                new StandStillBonus(PlayerStats.Instance().GetSpellPower() * DURATION_MULTIPLIER, false)
                );

            DialogUI.Instance.UpdateTextMessage("Captains now standing still.");
        }
    }
}