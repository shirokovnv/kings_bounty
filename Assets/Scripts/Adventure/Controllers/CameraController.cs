using UnityEngine;

namespace Assets.Scripts.Adventure.Controllers
{
    public class CameraController : MonoBehaviour
    {
        private static float offsetX = 0.5f;

        [SerializeField] private Transform target;

        void LateUpdate()
        {
            transform.position = new Vector3(
                target.transform.position.x + offsetX,
                target.transform.position.y,
                transform.position.z
                );
        }
    }
}