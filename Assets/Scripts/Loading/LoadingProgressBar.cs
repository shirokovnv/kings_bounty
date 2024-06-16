using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Loading
{
    public class LoadingProgressBar : MonoBehaviour
    {
        private Image image;

        private void Awake()
        {
            image = transform.GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            image.fillAmount = SceneLoader.GetLoadingProgress();
        }
    }
}