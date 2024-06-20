using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Accounting;
using Assets.Scripts.Combat;
using Assets.Scripts.Combat.Interfaces;
using Assets.Scripts.Combat.Logic.Systems;
using Assets.Scripts.Shared.Logic.Bonuses;
using Assets.Scripts.Shared.Logic.Bonuses.Modifiers;
using Assets.Scripts.Shared.Logic.Character.Ranks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Character
{
    [System.Serializable]
    public class PlayerSquads : ISquadable, ITaxable, ISerializationCallbackReceiver
    {
        private const int MAX_SQUADS = 5;
        private const int MIN_SQUADS = 1;

        private static PlayerSquads instance;

        private List<Modifier<float>> modifiers;
        [SerializeField] private List<UnitGroup> squads;
        [SerializeField] private List<BonusPackage> modPackages;

        public static PlayerSquads Instance()
        {
            instance ??= new PlayerSquads
            {
                squads = new List<UnitGroup>(),
                modifiers = new List<Modifier<float>>()
            };

            return instance;
        }

        public PlayerSquads AddSquad(UnitGroup squad)
        {
            if (squads.Count >= MAX_SQUADS)
            {
                return this;
            }

            squads.Add(squad);

            return this;
        }

        public void AddOrRefillSquad(UnitScriptableObject unit, int quantity)
        {
            AddOrRefillSquad(unit, quantity, UnitGroup.UnitOwner.player);
        }

        public void AddOrRefillSquad(UnitScriptableObject unit, int quantity, UnitGroup.UnitOwner owner)
        {
            var squad = squads.FirstOrDefault(s => s.Unit.Name == unit.Name);

            if (squads.Count >= MAX_SQUADS && squad == null)
            {
                return;
            }

            if (squad != null)
            {
                squad.SetQuantity(quantity + squad.CurrentQuantity());
            }
            else
            {
                squads.Add(new UnitGroup(unit, quantity, owner, 0, squads.Count));
            }

            MoraleSystem.Instance().CalculateMorale(squads);
        }

        public void CalculateModifiers()
        {
            var attackModifiers = modifiers.Where(m => m is AttackModifier).ToList();
            var defenceModifiers = modifiers.Where(m => m is DefenceModifier).ToList();

            squads.ForEach(s =>
            {
                s.DamageAmplificationCoefficient = s.DamageReductionCoefficient = 0;

                attackModifiers.ForEach(m => s.DamageAmplificationCoefficient += m.GetValue());
                defenceModifiers.ForEach(m => s.DamageReductionCoefficient += m.GetValue());
            });
        }

        public void RemoveSquad(int index)
        {
            if (index < 0 || index >= squads.Count) { return; }

            if (squads.Count == MIN_SQUADS) return;

            squads.RemoveAt(index);

            MoraleSystem.Instance().CalculateMorale(squads);
        }

        public List<UnitGroup> GetSquads()
        {
            return squads;
        }

        public bool CanHireSquad(UnitScriptableObject unit)
        {
            var squad = squads.FirstOrDefault(s => s.Unit.Name == unit.Name);

            return squad != null || squads.Count < MAX_SQUADS;
        }

        public void RemoveAllSquads()
        {
            squads.Clear();
        }

        public int GetMinSquads() { return MIN_SQUADS; }

        public int GetMaxSquads() { return MAX_SQUADS; }

        public void SetBaseSquads(IRankable rankable)
        {
            RemoveAllSquads();

            rankable.GetBaseArmy().ForEach(s =>
            {
                AddOrRefillSquad(s.Unit, s.CurrentQuantity());
            });
        }

        public int GetTax()
        {
            return squads.Aggregate(0, (acc, squad) => squad.GetTax() + acc);
        }

        public string GetTaxInfo()
        {
            return squads.Aggregate("", (acc, squad) => squad.GetTaxInfo() + "\r\n" + acc);
        }

        public void AddModifier(Modifier<float> modifier)
        {
            modifiers.Add(modifier);
        }

        public int GetSquadsTotalQuantity()
        {
            return squads.Aggregate(0, (acc, squad) => acc + squad.CurrentQuantity());
        }

        public void OnBeforeSerialize()
        {
            modPackages = new List<BonusPackage>(modifiers.Select(modifier => new BonusPackage(modifier)));
        }

        public void OnAfterDeserialize()
        {
            modifiers = new List<Modifier<float>>(modPackages.Select(package => package.GetBonus() as Modifier<float>));

            instance = this;
        }
    }
}