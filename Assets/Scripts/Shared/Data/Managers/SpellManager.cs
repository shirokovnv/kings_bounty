using Assets.Resources.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Shared.Data.Managers
{
    public class SpellManager
    {
        private static SpellManager instance;
        private List<SpellScriptableObject> spellScripts;

        public static SpellManager Instance()
        {
            instance ??= new SpellManager
            {
                spellScripts = UnityEngine.Resources.LoadAll<SpellScriptableObject>("ScriptableObjects").ToList()
            };

            return instance;
        }

        public List<SpellScriptableObject> GetSpellScripts() { return spellScripts; }
        public SpellScriptableObject GetRandomSpell()
        {
            return spellScripts[Random.Range(0, spellScripts.Count)];
        }

        public int GetRandomSpellIndex()
        {
            return Random.Range(0, spellScripts.Count);
        }

        public SpellScriptableObject GetSpellAt(int index)
        {
            if (index < 0 || index >= spellScripts.Count) { return null; }

            return spellScripts[index];
        }

        public SpellScriptableObject GetSpellByID(int id)
        {
            return spellScripts.Where(spell => spell.GetScriptID() == id).FirstOrDefault();
        }

        public SpellScriptableObject GetSpellByName(string name)
        {
            return spellScripts.Where(spell => spell.Name.Equals(name)).FirstOrDefault();
        }
    }
}