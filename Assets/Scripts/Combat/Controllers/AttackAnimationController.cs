using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using UnityEngine;

namespace Assets.Scripts.Combat.Controllers
{
    public class AttackAnimationController : MonoBehaviour
    {
        public static AttackAnimationController Instance;

        protected float timer;
        protected float frameRatio = 0.5f;
        protected bool isAnimated = false;

        private void Awake()
        {
            Instance = this;
            gameObject.transform.position = new Vector3(-10, -10);
        }

        // Update is called once per frame
        void Update()
        {
            if (isAnimated)
            {
                timer += Time.deltaTime;

                if (timer > frameRatio)
                {
                    timer = 0;
                    isAnimated = false;
                }
            }
        }

        public void BeginAnimation(IAttackAction action)
        {
            isAnimated = true;
            timer = 0;

            transform.localPosition = new Vector3(action.Defender().X * 2, action.Defender().Y * 2);
        }

        public void EndAnimation()
        {
            isAnimated = false;
            timer = 0;

            transform.localPosition = new Vector3(-10, -10);
        }
    }
}