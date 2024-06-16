using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Logic.Character.Ranks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Adventure.UI.InventoryPanel
{
    public class InventoryPanelScript : MonoBehaviour
    {
        private const int INVENTORY_SIZE = 12;

        private static Dictionary<string, Func<int>> STATS = new()
    {
        { "Leadership", PlayerStats.Instance().GetLeadership },
        { "Commission/Week", PlayerStats.Instance().GetWeekSalary },
        { "Gold", PlayerStats.Instance().GetGold },
        { "Spell power", PlayerStats.Instance().GetSpellPower },
        { "Knowledge", PlayerStats.Instance().GetKnowledge },
        { "Villains caught", PlayerStats.Instance().GetVillainsCaught },
        { "Artifacts found", PlayerStats.Instance().GetArtifactsFound },
        { "Castles garrisoned", PlayerStats.Instance().GetCastlesGarrisoned },
        { "Followers killed", PlayerStats.Instance().GetFollowersKilled },
        { "Score", PlayerStats.Instance().GetScore }
    };

        [SerializeField] private GameObject heroPortrait;
        [SerializeField] private GameObject heroStats;
        [SerializeField] private GameObject inventory;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Sprite[] heroSprites;

        private static List<GameObject> inventoryItems;

        private void Awake()
        {
            gameObject.SetActive(false);
            inventoryItems = new();

            EventBus.Instance.Register(this);
        }

        public void OnEvent(OnViewInventory e)
        {
            var (titles, values) = PrepareStatsInfo();

            Text[] texts = heroStats.GetComponentsInChildren<Text>();

            texts[0].text = titles;
            texts[1].text = values;

            heroPortrait.GetComponent<Image>().sprite = GetHeroSprite(Player.Instance().GetRank());

            CreateInventory();

            gameObject.SetActive(true);
        }

        public void OnEvent(OnHideInventory e)
        {
            DestroyInventory();

            gameObject.SetActive(false);
        }

        private (string, string) PrepareStatsInfo()
        {
            var player = Player.Instance();
            PlayerStats stats = PlayerStats.Instance();

            string titles = string.Empty,
                   values = string.Empty;

            titles += $"<i><size=40>{player.GetFullTitle()}</size></i> \r\n";
            titles += "\r\n";

            values += "\r\n \r\n";

            foreach (var (title, callback) in STATS)
            {
                titles += $"{title} \r\n";
                values += $"{callback()} \r\n";
            }

            return (titles, values);
        }

        private void CreateInventory()
        {
            var artifacts = Inventory.Instance().GetArtifacts();

            // TODO: better solution?
            if (Screen.width == 800 && Screen.height == 600)
            {
                inventory.GetComponent<GridLayoutGroup>().cellSize = new Vector2(230, 160);
            }

            for (int i = 0; i < INVENTORY_SIZE; i++)
            {
                var item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(inventory.transform);
                item.transform.localScale = new Vector3(1f, 1f, 1f);

                if (i < artifacts.Count)
                {
                    item.GetComponent<Image>().sprite = artifacts.ElementAt(i).ArtifactScript.InventorySprite;
                }

                inventoryItems.Add(item);
            }
        }

        private void DestroyInventory()
        {
            inventoryItems.ForEach(item => Destroy(item));
            inventoryItems.Clear();
        }

        private Sprite GetHeroSprite(Rank rank)
        {
            return rank switch
            {
                Knight => heroSprites[0],
                Ranger => heroSprites[1],
                Sorceress => heroSprites[2],
                _ => null,
            };
        }
    }
}