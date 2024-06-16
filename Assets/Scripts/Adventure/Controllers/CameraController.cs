using UnityEngine;

namespace Assets.Scripts.Adventure.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;

        void LateUpdate()
        {
            transform.position = new Vector3(
                target.transform.position.x,
                target.transform.position.y,
                transform.position.z
                );
        }
    }
}