using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character.Spells;
using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Character
{
    [System.Serializable]
    public class Spellbook : ISerializationCallbackReceiver
    {
        [System.Serializable]
        struct SpellPackage
        {
            public int SpellID;
            public int Count;
        }

        [SerializeField] private List<SpellPackage> spellPackages;

        private Dictionary<SpellScriptableObject, int> travelSpells;
        private Dictionary<SpellScriptableObject, int> combatSpells;

        private static Spellbook instance;

        private Spellbook()
        {
        }

        public static Spellbook Instance()
        {
            if (instance == null)
            {
                instance = new();
                instance.MakeSpellbookRecipes();
            }

            return instance;
        }

        public Dictionary<SpellScriptableObject, int> GetTravelSpells()
        {
            return travelSpells;
        }

        public Dictionary<SpellScriptableObject, int> GetCombatSpells()
        {
            return combatSpells;
        }

        public int TotalNumberOfSpells()
        {
            return travelSpells.Sum(x => x.Value) + combatSpells.Sum(x => x.Value);
        }

        public void AddSpell(SpellScriptableObject spell, int count = 1)
        {
            var spellDict = spell.Group == SpellScriptableObject.SpellGroup.travel
                ? travelSpells
                : combatSpells;

            spellDict[spell] += count;
        }

        public void UseSpell(SpellScriptableObject spell, SpellTarget target = null)
        {
            var spellDict = spell.Group == SpellScriptableObject.SpellGroup.travel
                ? travelSpells
                : combatSpells;

            if (spellDict[spell] == 0)
            {
                return;
            }

            spellDict[spell]--;

            var castable = CastableFactory.Create(spell, target);

            castable.Cast();

            EventBus.Instance.PostEvent(new OnCastSpell());
        }

        public void OnBeforeSerialize()
        {
            spellPackages = new List<SpellPackage>();

            var travelPackage = travelSpells.Select(spellKvp => new SpellPackage
            {
                SpellID = spellKvp.Key.GetScriptID(),
                Count = spellKvp.Value
            });

            var combatPackage = combatSpells.Select(spellKvp => new SpellPackage
            {
                SpellID = spellKvp.Key.GetScriptID(),
                Count = spellKvp.Value
            });

            spellPackages.AddRange(travelPackage.Concat(combatPackage));
        }

        public void OnAfterDeserialize()
        {
            instance = this;

            MakeSpellbookRecipes();

            foreach (var spellPackage in spellPackages)
            {
                AddSpell(
                    SpellManager.Instance().GetSpellByID(spellPackage.SpellID),
                    spellPackage.Count
                    );
            }
        }

        private void MakeSpellbookRecipes()
        {
            travelSpells = new();
            combatSpells = new();

            var spells = SpellManager.Instance().GetSpellScripts();

            spells
                .Where(spell => spell.Group == SpellScriptableObject.SpellGroup.travel)
                .ToList()
                .ForEach(spell => travelSpells.Add(spell, 0));

            spells
                .Where(spell => spell.Group == SpellScriptableObject.SpellGroup.combat)
                .ToList()
                .ForEach(spell => combatSpells.Add(spell, 0));
        }
    }
}