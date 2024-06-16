using Assets.Scripts.Loading;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Data.State.MainMenu;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenu
{
    public class LoadGameScript : MonoBehaviour
    {
        private string fileName;

        public void OnClick()
        {
            GameStateManager.Instance().SetState(new LoadGame { FileName = fileName });

            SceneLoader.Load(SceneLoader.Scene.AdventureScene);
        }

        public void SetFileName(string fileName)
        {
            this.fileName = fileName;

            GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(fileName);
        }
    }
}