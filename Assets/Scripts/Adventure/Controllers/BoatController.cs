using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using UnityEngine;

namespace Assets.Scripts.Adventure.Controllers
{
    public class BoatController : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(Boat.Instance().IsActive);
            gameObject.transform.position = new Vector3(Boat.Instance().X, Boat.Instance().Y);

            EventBus.Instance.Register(this);
        }

        public void OnEvent(OnBuyBoat e)
        {
            gameObject.transform.position = new Vector3(e.X, e.Y);
            gameObject.SetActive(true);
        }

        public void OnEvent(OnCancelBoat e)
        {
            gameObject.SetActive(false);
        }

        public void OnEvent(OnShore e)
        {
            gameObject.SetActive(true);
        }

        public void OnEvent(OnBoat e)
        {
            gameObject.SetActive(false);
        }

        public void OnEvent(OnChangePosition e)
        {
            gameObject.SetActive(Boat.Instance().IsActive);

            if (Player.Instance().IsNavigating())
            {
                gameObject.transform.position = new Vector3(e.X, e.Y);
            }
        }
    }
}