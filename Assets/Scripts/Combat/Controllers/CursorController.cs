using UnityEngine;

namespace Assets.Scripts.Combat.Controllers
{
    public class CursorController : MonoBehaviour
    {
        public static CursorController Instance;

        [SerializeField] private Sprite[] sprites;

        protected float frameRatio = 0.2f;
        protected float timer = 0;
        protected int currentFrame;

        protected int width, height;
        protected int cursorX, cursorY;

        private void Awake()
        {
            Instance = this;
            cursorX = 0;
            cursorY = 0;

            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;

            if (timer > frameRatio)
            {
                timer -= frameRatio;

                currentFrame = (currentFrame + 1) % sprites.Length;

                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[currentFrame];
            }

            ProcessUserInput();
        }

        public void ProcessUserInput()
        {
            int dX = -1;
            int dY = -1;

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                dX = cursorX - 1;
                dY = cursorY;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                dX = cursorX + 1;
                dY = cursorY;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                dX = cursorX;
                dY = cursorY + 1;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                dX = cursorX;
                dY = cursorY - 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                dX = cursorX - 1;
                dY = cursorY - 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                dX = cursorX + 1;
                dY = cursorY - 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                dX = cursorX - 1;
                dY = cursorY + 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                dX = cursorX + 1;
                dY = cursorY + 1;
            }

            // check edges (7, 5)
            if (dX < 0 || dY < 0 || dX >= width || dY >= height)
            {
                return;
            }

            cursorX = dX;
            cursorY = dY;

            transform.localPosition = new Vector3(dX * 2, dY * 2);
        }

        public void ActivateWithPosition(int cursorX, int cursorY, int width, int height)
        {
            gameObject.SetActive(true);
            this.cursorX = cursorX;
            this.cursorY = cursorY;
            this.width = width;
            this.height = height;

            transform.localPosition = new Vector3(cursorX * 2, cursorY * 2);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public int GetCursorX() { return cursorX; }
        public int GetCursorY() { return cursorY; }
    }
}