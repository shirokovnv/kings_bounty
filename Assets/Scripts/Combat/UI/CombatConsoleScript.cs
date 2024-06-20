using Assets.Scripts.Combat.Events;
using Assets.Scripts.Shared.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Combat.UI
{
    public class CombatConsoleScript : MonoBehaviour
    {
        [SerializeField] private bool showConsole;
        [SerializeField] private int consoleHeight;
        [SerializeField] private GameObject toggleButton;
        [SerializeField] private GameObject scrollView;
        [SerializeField] private GameObject content;
        [SerializeField] private GameObject consoleTextPrefab;

        private Stack<GameObject> stack;

        public static CombatConsoleScript Instance;

        private void Awake()
        {
            Instance = this;
            stack = new Stack<GameObject>();

            EventBus.Instance.Register(this);

            SetToggleButtonText();
        }

        private void Start()
        {
            scrollView.SetActive(showConsole);
            CalculateToggleButtonPosition();
        }

        public void OnToggleButtonClick()
        {
            showConsole = !showConsole;

            CalculateToggleButtonPosition();
            SetToggleButtonText();

            scrollView.SetActive(showConsole);
        }

        public void PushMessage(string text)
        {
            var newTextObj = Instantiate(consoleTextPrefab, Vector3.zero, Quaternion.identity);

            newTextObj.GetComponent<Text>().text = text;
            newTextObj.transform.SetParent(content.transform, false);
            newTextObj.transform.SetSiblingIndex(0);
            stack.Push(newTextObj);
        }

        public void ClearConsole()
        {
            while (stack.Count > 0)
            {
                Destroy(stack.Pop());
            }
        }

        public void OnEvent(OnActionExecute e)
        {
            string message = e.CombatAction.Message();
            if (message != string.Empty)
            {
                PushMessage(message);
            }
        }

        private void SetToggleButtonText()
        {
            string toggleText = showConsole
                ? "(X) Close"
                : "(O) Open";

            toggleButton.GetComponentInChildren<Text>().text = toggleText;
        }

        private void CalculateToggleButtonPosition()
        {
            toggleButton.transform.position = showConsole
                ? new Vector3(0, consoleHeight, 0)
                : new Vector3(0, 0, 0);
        }
    }
}