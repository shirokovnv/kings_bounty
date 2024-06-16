using Assets.Scripts.Shared.Logic.Systems;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class LoadSavedGameScript : MonoBehaviour
    {
        public static LoadSavedGameScript Instance;

        [SerializeField] private LoadGameScript loadGamePrefab;

        private List<GameObject> savedGames;

        private void Awake()
        {
            Instance = this;

            savedGames = new List<GameObject>();
        }

        public void ViewAllSavedGames()
        {
            foreach (var savedGame in savedGames)
            {
                Destroy(savedGame);
            }

            savedGames.Clear();

            var savedFiles = System.IO.Directory.GetFiles(FileSystem.SAVE_DIR, "*.txt");

            foreach (var savedFile in savedFiles)
            {
                var loadGameObject = Instantiate(loadGamePrefab, Vector3.zero, Quaternion.identity);
                loadGameObject.transform.SetParent(transform, false);
                loadGameObject.SetFileName(savedFile);

                savedGames.Add(loadGameObject.gameObject);
            }
        }
    }
}