using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Adventure.UI.Dialog
{
    public class DialogUI : MonoBehaviour
    {
        public static DialogUI Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            HideDialog();
        }

        public void HideDialog()
        {
            gameObject.SetActive(false);
        }

        public void ShowDialogUI(string title, string text, Sprite sprite)
        {
            transform.Find("DialogTitle").GetComponent<Text>().text = title;
            transform.Find("DialogContent").GetComponent<Text>().text = text;
            transform.Find("DialogImage").GetComponent<Image>().sprite = sprite;

            gameObject.SetActive(true);
        }

        public void UpdateTextMessage(string text)
        {
            transform.Find("DialogContent").GetComponent<Text>().text = text;
        }
    }
}