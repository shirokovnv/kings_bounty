using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Adventure.UI.BottomPanel
{
    public class BottomUIScript : MonoBehaviour
    {
        [SerializeField] private GameObject titleObject;
        [SerializeField] private GameObject optionsObject;
        [SerializeField] private GameObject userInputObject;
        [SerializeField] private GameObject topRightTextObject;
        [SerializeField] private GameObject bottomRightTextObject;

        public static BottomUIScript Instance;

        private void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);
        }

        public void ShowBottomUI()
        {
            gameObject.SetActive(true);
        }

        public void HideBottomUI()
        {
            gameObject.SetActive(false);
        }

        public void ShowUIMessage(
            string title,
            string options,
            string topRightText = null,
            string bottomRightText = null,
            bool activateInput = false
            )
        {
            titleObject.GetComponent<Text>().text = title;
            optionsObject.GetComponent<Text>().text = options;

            userInputObject.SetActive(activateInput);

            if (topRightText != null)
            {
                topRightTextObject.SetActive(true);
                topRightTextObject.GetComponent<Text>().text = topRightText;
            }
            else
            {
                topRightTextObject.SetActive(false);
            }

            if (bottomRightText != null)
            {
                bottomRightTextObject.SetActive(true);
                bottomRightTextObject.GetComponent<Text>().text = bottomRightText;
            }
            else
            {
                bottomRightTextObject.SetActive(false);
            }
        }

        public void UpdateTextMessage(string text)
        {
            optionsObject.GetComponent<Text>().text = text;
        }
    }
}