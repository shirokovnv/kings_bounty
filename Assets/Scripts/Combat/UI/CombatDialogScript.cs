using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Combat.UI
{
    public class CombatDialogScript : MonoBehaviour
    {
        [SerializeField] private GameObject TitleObject;
        [SerializeField] private GameObject ContentObject;

        public static CombatDialogScript Instance;

        private void Awake()
        {
            Instance = this;

            gameObject.SetActive(false);
        }

        public void ShowDialog(string title, string content)
        {
            TitleObject.GetComponent<Text>().text = title;
            ContentObject.GetComponent<Text>().text = content;

            gameObject.SetActive(true);
        }

        public void HideDialog()
        {
            gameObject.SetActive(false);
        }
    }
}