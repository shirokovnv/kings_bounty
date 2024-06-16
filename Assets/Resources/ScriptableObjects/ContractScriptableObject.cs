using UnityEngine;

namespace Assets.Resources.ScriptableObjects
{
    [CreateAssetMenu(fileName = "contract", menuName = "ScriptableObjects/contract")]
    public class ContractScriptableObject : ScriptableObject
    {
        public string Title;
        public string Alias;
        [TextArea] public string DistinguishingFeatures;
        [TextArea] public string Crimes;

        public Sprite[] Sprites;

        public int GetScriptID()
        {
            return $"CONTRACT__{Title}#{Alias}".GetHashCode();
        }
    }
}