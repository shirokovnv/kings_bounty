using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Events;
using UnityEngine;

namespace Assets.Scripts.Adventure.UI.RightPanel
{
    public class SpellPowerUIScript : AbstractUIBehaviour
    {
        [SerializeField] private Sprite[] spellPowerSprites;

        private void Awake()
        {
            EventBus.Instance.Register(this);
        }

        public void OnEvent(OnChangeSpellPower e)
        {
            sprites = e.Power > 0 ? spellPowerSprites : null;
        }
    }
}