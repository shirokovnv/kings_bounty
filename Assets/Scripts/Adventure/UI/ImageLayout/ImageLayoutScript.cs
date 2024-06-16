using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Adventure.UI.ImageLayout
{
    public class ImageLayoutScript : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;

        protected const float frameRatio = 0.2f;

        protected int currentFrame;
        protected float timer;

        private void Awake()
        {
            EventBus.Instance.Register(this);
            gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Screen: " + Screen.width);
            Debug.Log(transform.position);
            //transform.position = new Vector3(- (Screen.width / 4), -30, 0);
        }

        // Update is called once per frame
        void Update()
        {
            if (sprites.Length == 0)
            {
                return;
            }

            timer += Time.deltaTime;

            if (timer > frameRatio)
            {
                timer -= frameRatio;
                currentFrame = (currentFrame + 1) % sprites.Length;

                gameObject.GetComponent<Image>().sprite = sprites[currentFrame];
            }
        }

        public void OnEvent(OnEnterTown e)
        {
            gameObject.SetActive(true);

            var unit = UnitManager.Instance().GetRandomUnit();
            sprites = unit.Sprites;
        }

        public void OnEvent(OnLeaveTown e)
        {
            gameObject.SetActive(false);
        }

        public void OnEvent(OnEnterDwelling e)
        {
            gameObject.SetActive(true);

            sprites = e.Dwelling.Unit.Sprites;
        }

        public void OnEvent(OnLeaveDwelling e)
        {
            gameObject.SetActive(false);
        }
    }
}