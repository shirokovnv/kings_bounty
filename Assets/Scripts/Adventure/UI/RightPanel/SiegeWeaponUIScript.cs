using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Events;
using UnityEngine;

namespace Assets.Scripts.Adventure.UI.RightPanel
{
    public class SiegeWeaponUIScript : AbstractUIBehaviour
    {
        [SerializeField] private Sprite[] siegeWeaponSprites;

        private void Awake()
        {
            EventBus.Instance.Register(this);
        }

        public void OnEvent(OnBuySiegeWeaponEvent e)
        {
            sprites = e.IsPurchased ? siegeWeaponSprites : null;
        }
    }
}