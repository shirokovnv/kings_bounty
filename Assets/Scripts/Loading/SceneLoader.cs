using Assets.Scripts.Shared.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Loading
{
    public static class SceneLoader
    {
        private class LoadingBehavior : MonoBehaviour { }

        public enum Scene
        {
            MainMenuScene,
            LoadingScene,
            AdventureScene,
            CombatScene,
            EndingScene,
        }

        private static Action onLoaderCallback;
        private static AsyncOperation asyncOperation;

        public static void Load(Scene scene)
        {
            EventBus.Instance.UnregisterAll();

            onLoaderCallback = () =>
            {
                GameObject loadingObject = new GameObject("Loading Game Object");
                loadingObject.AddComponent<LoadingBehavior>().StartCoroutine(LoadSceneAsync(scene));
            };

            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

        public static void LoaderCallback()
        {
            if (onLoaderCallback != null)
            {
                onLoaderCallback();
                onLoaderCallback = null;
            }
        }

        public static float GetLoadingProgress()
        {
            if (asyncOperation != null)
            {
                return asyncOperation.progress;
            }

            return 1f;
        }

        private static IEnumerator LoadSceneAsync(Scene scene)
        {
            yield return null;

            asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

            while (!asyncOperation.isDone)
            {
                yield return null;
            }

        }
    }
}