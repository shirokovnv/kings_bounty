using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Adventure.UI.Background
{
    public class BackgroundUIScript : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        private Image backgroundImage;
        private Sprite backgroundSprite;

        public static BackgroundUIScript Instance;

        private void Awake()
        {
            Instance = this;
            backgroundImage = GetComponent<Image>();

            gameObject.SetActive(false);
        }

        public void ShowObjectBackground(BaseObject obj)
        {
            int spriteIndex = GetSpriteIndexForObject(obj);

            if (spriteIndex == -1)
            {
                return;
            }

            backgroundSprite = sprites[spriteIndex];

            if (backgroundSprite != null)
            {
                gameObject.SetActive(true);
                backgroundImage.sprite = backgroundSprite;
                backgroundImage.color = new Color(1, 1, 1, 1);
            }
        }

        public void HideBackground()
        {
            gameObject.SetActive(false);
        }

        private int GetSpriteIndexForObject(BaseObject obj)
        {
            switch (obj.GetObjectType())
            {
                case ObjectType.castleGate: return 1;
                case ObjectType.city: return 5;
                case ObjectType.dwelling:
                    var dwelling = obj as Dwelling;
                    switch (dwelling.GetDwellingType())
                    {
                        case Dwelling.DwellingType.plains: return 4;
                        case Dwelling.DwellingType.forest: return 3;
                        case Dwelling.DwellingType.dungeon: return 2;
                        case Dwelling.DwellingType.cavern: return 0;
                    }
                    break;
            }

            return -1;
        }
    }
}