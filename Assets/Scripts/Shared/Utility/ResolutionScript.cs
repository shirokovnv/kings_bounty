using UnityEngine;

namespace Assets.Scripts.Shared.Utility
{
    public class ResolutionScript : MonoBehaviour
    {
        [SerializeField] private int newResolutionWidth;
        [SerializeField] private int newResolutionHeight;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (Screen.width != newResolutionWidth && Screen.height != newResolutionHeight)
            {
                Screen.SetResolution(newResolutionWidth, newResolutionHeight, false);
            }
        }
    }
}