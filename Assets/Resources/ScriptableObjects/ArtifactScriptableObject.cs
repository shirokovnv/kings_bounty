using UnityEngine;

namespace Assets.Resources.ScriptableObjects
{
    [CreateAssetMenu(fileName = "artifact", menuName = "ScriptableObjects/artifact")]
    public class ArtifactScriptableObject : ScriptableObject
    {
        public string Name;
        public string Description;
        public int Level;

        public Sprite WorldSprite;
        public Sprite InventorySprite;

        public int GetScriptID()
        {
            return ("ARTIFACT__" + Name).GetHashCode();
        }
    }
}