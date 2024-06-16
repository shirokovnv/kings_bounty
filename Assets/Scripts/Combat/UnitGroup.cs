using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Accounting;
using Assets.Scripts.Combat.Interfaces;
using Assets.Scripts.Combat.Logic.Systems;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Logic.Character;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    [System.Serializable]
    public class UnitGroup :
        ITaxable,
        IMessageable,
        ISerializationCallbackReceiver
    {
        private const int WEEK_COST_PENALTY = 10;

        public enum UnitOwner
        {
            player,
            opponent,
        }

        [SerializeField] private int UnitID;

        [System.NonSerialized] public UnitScriptableObject Unit;
        public int InitialQuantity;
        public UnitOwner Owner;

        public int InitialHP;
        public int CurrentHP;

        public int CurrentMoves;
        public int CurrentShoots;
        public int CurrentCounterstrikes;

        public int X, Y;

        public MoraleType Morale;
        public bool IsPetrified;

        public float DamageReductionCoefficient;
        public float DamageAmplificationCoefficient;

        public UnitGroup(
            UnitScriptableObject unit,
            int initialQuantity,
            UnitOwner owner,
            int x,
            int y
            )
        {
            Unit = unit;
            Owner = owner;
            X = x;
            Y = y;
            IsPetrified = false;

            SetQuantity(initialQuantity);

            ResetMovement();
            ResetShoots();
            ResetCounterstrikes();

            DamageAmplificationCoefficient = DamageReductionCoefficient = 0;
        }

        public void SetQuantity(int quantity)
        {
            InitialQuantity = quantity;
            InitialHP = CurrentHP = Unit.HP * quantity;
        }

        public int CurrentQuantity()
        {
            int quantity = CurrentHP / Unit.HP;
            int remainder = CurrentHP % Unit.HP;

            return quantity + (remainder > 0 ? 1 : 0);
        }

        public void ApplyDamage(int damage)
        {
            CurrentHP -= damage;
        }

        public int MeleeDamage()
        {
            return Random.Range(Unit.MinDamage, Unit.MaxDamage + 1) * CurrentQuantity();
        }

        public int RangedDamage()
        {
            return Random.Range(Unit.MinRangeDamage, Unit.MaxRangeDamage + 1) * CurrentQuantity();
        }

        public bool IsDead()
        {
            return CurrentHP <= 0;
        }

        public bool IsUndead()
        {
            return Unit.MoraleGroup == MoraleGroup.E;
        }

        public bool IsRanged()
        {
            return Unit.NumShoots > 0;
        }

        public bool IsFlyer()
        {
            return Unit.CanFly;
        }

        public void ResetMovement()
        {
            CurrentMoves = Unit.NumMoves;
        }

        public void ResetCounterstrikes()
        {
            CurrentCounterstrikes = Unit.NumCounterstrikes;
        }

        public void ResetShoots()
        {
            CurrentShoots = Unit.NumShoots;
        }

        public int GetTax()
        {
            return Unit.Cost * CurrentQuantity() / WEEK_COST_PENALTY;
        }

        public int CurrentMinDamage()
        {
            return Unit.MinDamage * CurrentQuantity();
        }

        public int CurrentMaxDamage()
        {
            return Unit.MaxDamage * CurrentQuantity();
        }

        public void RegenerateToFullHP()
        {
            CurrentHP = Unit.HP * CurrentQuantity();
        }

        public void RestoreHP(int amount)
        {
            int newHP = Mathf.Min(InitialHP, CurrentHP + amount);

            CurrentHP = newHP;
        }

        public void AddHP(int amount)
        {
            CurrentHP += amount;
        }

        public override string ToString()
        {
            return Unit.Name + ": "
                + CurrentQuantity()
                + " x=" + X
                + " y=" + Y
                + "\n";
        }

        public string GetTaxInfo()
        {
            return Unit.Name + ": " + GetTax();
        }

        public void OnBeforeSerialize()
        {
            UnitID = Unit.GetScriptID();
        }

        public void OnAfterDeserialize()
        {
            Unit = UnitManager.Instance().GetUnitByID(UnitID);
        }

        public string Message()
        {
            return $"{Unit.Name} ({Owner}) ({X}, {Y})";
        }

        public bool IsOutOfControl()
        {
            return Owner == UnitOwner.player &&
                Unit.HP * CurrentQuantity() > PlayerStats.Instance().GetLeadership();
        }
    }
}