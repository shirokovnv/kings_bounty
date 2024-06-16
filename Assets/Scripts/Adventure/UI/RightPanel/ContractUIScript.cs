using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Events;

namespace Assets.Scripts.Adventure.UI.RightPanel
{
    public class ContractUIScript : AbstractUIBehaviour
    {
        private void Awake()
        {
            EventBus.Instance.Register(this);
        }

        public void OnEvent(OnNewContract e)
        {
            sprites = e.Contract?.ContractInfo().Sprites;
        }
    }
}