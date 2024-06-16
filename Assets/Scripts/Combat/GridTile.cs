using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class GridTile : MonoBehaviour
    {
        private const int TEXT_MESH_ORDER = 300;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject textMesh;

        protected bool isAnimated = false;
        protected Sprite[] animatedSprites;

        protected const float frameRatio = 0.2f;
        protected int currentFrame;
        protected float timer;

        private void Awake()
        {
            textMesh.GetComponent<MeshRenderer>().sortingOrder = TEXT_MESH_ORDER;
        }

        private void Update()
        {
            textMesh.GetComponent<MeshRenderer>().sortingOrder = 300;

            if (isAnimated && animatedSprites != null)
            {
                timer += Time.deltaTime;

                if (timer > frameRatio)
                {
                    timer -= frameRatio;
                    currentFrame = (currentFrame + 1) % animatedSprites.Length;

                    spriteRenderer.sprite = animatedSprites[currentFrame];
                }
            }
        }

        public void Init(
            Sprite sprite = null,
            bool hideText = false,
            bool flip = false,
            int? sortingOrder = null
            )
        {
            if (sprite != null)
            {
                spriteRenderer.sprite = sprite;

                if (sortingOrder != null)
                {
                    spriteRenderer.sortingOrder = (int)sortingOrder;
                }
            }

            if (hideText)
            {
                textMesh.SetActive(false);
            }

            if (flip)
            {
                TextMesh mesh = textMesh.GetComponent<TextMesh>();

                Vector3 meshScale = mesh.transform.localScale;
                meshScale.x *= -1;

                mesh.transform.localScale = meshScale;
                mesh.anchor = TextAnchor.LowerLeft;

                Vector3 spriteScale = spriteRenderer.transform.localScale;
                spriteScale.x *= -1;
                spriteRenderer.transform.localScale = spriteScale;
            }
        }

        public void SetSpriteScaleRatio(float scaleX, float scaleY)
        {
            spriteRenderer.size = new Vector2Int(1, 1);

            Vector3 scale = spriteRenderer.transform.localScale;
            scale.x *= scaleX;
            scale.y *= scaleY;
            spriteRenderer.transform.localScale = scale;
        }

        public void SetSpritePosition(Vector2 position)
        {
            spriteRenderer.transform.localPosition = position;
        }

        public void SetText(string text)
        {
            textMesh.GetComponent<TextMesh>().text = text;
        }

        public void SetSortingOrder(int sortingOrder)
        {
            spriteRenderer.sortingOrder = sortingOrder;
        }

        public void SetColor(Color32 color)
        {
            spriteRenderer.color = color;
        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        public void SetAnimatedSprites(Sprite[] sprites)
        {
            animatedSprites = sprites;
        }

        public void SetTextMeshActive(bool active)
        {
            textMesh.SetActive(active);
        }

        public void SetLocalScale(Vector3 scale)
        {
            transform.localScale = scale;
        }

        public void BeginAnimation(Sprite[] animatedSprites)
        {
            currentFrame = 0;
            isAnimated = true;
            this.animatedSprites = animatedSprites;
        }

        public void EndAnimation()
        {
            currentFrame = 0;
            spriteRenderer.sprite = animatedSprites?[0];
            isAnimated = false;
        }

        public bool IsAnimated() { return isAnimated; }
    }
}