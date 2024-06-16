using UnityEngine;

namespace Assets.Scripts.Shared.Utility
{
    public class ResolutionScript : MonoBehaviour
    {
        [SerializeField] private int newResolutionWidth;
        [SerializeField] private int newResolutionHeight;

        private void Awake()
        {
            Screen.SetResolution(newResolutionWidth, newResolutionHeight, false);
        }
    }
}