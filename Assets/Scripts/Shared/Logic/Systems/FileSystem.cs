using Assets.Scripts.Shared.Logic.Character;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Systems
{
    public static class FileSystem
    {
        public static readonly string SAVE_DIR = Application.dataPath + "/Saves/";

        public const string SAVE_EXTENSION = "sav";

        public static void Save()
        {
            var saveFileInfo = new SaveFileInfo();

            var str = JsonUtility.ToJson(saveFileInfo);

            if (!Directory.Exists(SAVE_DIR))
            {
                Directory.CreateDirectory(SAVE_DIR);
            }

            File.WriteAllBytes(
                $"{SAVE_DIR}{Player.Instance().GetName()}.{SAVE_EXTENSION}",
                FileCompressor.Zip(str)
            );

            Debug.Log("SAVED!");
        }

        public static bool Load(string path)
        {
            if (File.Exists(path))
            {
                string saveStr = FileCompressor.Unzip(File.ReadAllBytes(path));

                JsonUtility.FromJson<SaveFileInfo>(saveStr);

                return true;
            }

            return false;
        }

        public static bool Exists(string path)
        {
            return File.Exists(path); 
        }

        public static string[] GetSavedFiles()
        {
            return Directory.GetFiles(SAVE_DIR, $"*.{SAVE_EXTENSION}");
        }
    }
}