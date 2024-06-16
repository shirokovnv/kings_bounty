using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources.ScriptableObjects
{
    [CreateAssetMenu(fileName = "spell", menuName = "ScriptableObjects/spell")]
    public class SpellScriptableObject : ScriptableObject
    {
        public enum SpellGroup
        {
            travel,
            combat,
        }

        public string Name;
        [TextArea] public string Description;
        public int Cost;
        public SpellGroup Group;

        public int GetScriptID()
        {
            return $"SPELL__{Group}#{Name}".GetHashCode();
        }
    }

    public class SpellScriptableObjectEqualityComparer : IEqualityComparer<SpellScriptableObject>
    {
        public bool Equals(SpellScriptableObject x, SpellScriptableObject y)
        {
            return x.Name == y.Name && x.Group == y.Group;
        }

        public int GetHashCode(SpellScriptableObject obj)
        {
            return obj.Name.GetHashCode() ^ obj.Group.GetHashCode();
        }
    }
}

