using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Adventure.UI.RightPanel
{
    abstract public class AbstractUIBehaviour : MonoBehaviour
    {
        protected const float frameRatio = 0.2f;

        protected Sprite[] sprites;

        [SerializeField] protected Sprite defaultSprite;

        protected int currentFrame;
        protected float timer;

        void Update()
        {
            if (sprites == null)
            {
                gameObject.GetComponent<Image>().sprite = defaultSprite;

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

    }
}