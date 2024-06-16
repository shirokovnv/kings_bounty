using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Events;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Adventure.UI.BottomPanel
{
    public class UserInputController : MonoBehaviour
    {
        private System.Action<int> purchaseCallback;

        private void Awake()
        {
            EventBus.Instance.Register(this);

            gameObject.GetComponent<InputField>().onValidateInput += ValidateInput;
        }

        public void OnEvent(OnEnterDwelling e)
        {
            purchaseCallback = e.PurchaseCallback;
        }

        public char ValidateInput(string text, int charIndex, char addedChar)
        {
            char output = addedChar;

            char[] allowedChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            if (text.Length > 0 && text.ElementAt(0) == allowedChars[0] ||
                !allowedChars.Contains(addedChar))
            {
                //return a null character
                output = '\0';
            }

            return output;
        }

        public void OnSubmit(string input)
        {
            if (int.TryParse(input, out int result) && purchaseCallback != null)
            {
                gameObject.GetComponent<InputField>().text = null;

                purchaseCallback(result);
            }
        }
    }
}