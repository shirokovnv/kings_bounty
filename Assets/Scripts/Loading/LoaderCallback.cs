using UnityEngine;

namespace Assets.Scripts.Loading
{
    public class LoaderCallback : MonoBehaviour
    {
        private bool isFirstUpdate = true;

        // Update is called once per frame
        void Update()
        {
            if (isFirstUpdate)
            {
                isFirstUpdate = false;
                SceneLoader.LoaderCallback();
            }
        }
    }
}