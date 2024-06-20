using Assets.Scripts.Shared.Logic.Character.Ranks;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Character
{
    [System.Serializable]
    public class Player : ISerializationCallbackReceiver
    {
        public enum Direction
        {
            left,
            right
        }

        public enum WalkMode
        {
            Normal,
            Levitate,
        }

        private static Player instance;
        private Rank rank;

        [SerializeField] private RankPackage rankPackage;
        [SerializeField] private string name;
        [SerializeField] private bool hasSiegeWeapon;
        [SerializeField] private bool isNavigating;
        [SerializeField] private int endurance;
        [SerializeField] private Direction direction;
        [SerializeField] private WalkMode walkMode;

        public int X, Y, ContinentNumber;

        private Player()
        {
        }

        public static Player Instance()
        {
            instance ??= new Player();

            return instance;
        }

        public string GetName()
        {
            return name;
        }

        public Rank GetRank()
        {
            return rank;
        }

        public string GetFullTitle()
        {
            return $"{GetName()} The {GetRank().GetRankName()}";
        }

        public void SetRank(Rank rank)
        {
            this.rank = rank;
        }

        public bool HasSiegeWeapon()
        {
            return hasSiegeWeapon;
        }

        public void SetHasSiegeWeapon(bool value)
        {
            hasSiegeWeapon = value;
        }

        public bool IsNavigating()
        {
            return isNavigating;
        }
        public void SetNavigating(bool isNavigating)
        {
            this.isNavigating = isNavigating;
        }

        public Direction GetDirection() { return direction; }

        public void SetDirection(Direction direction)
        {
            this.direction = direction;
        }

        public WalkMode GetWalkMode()
        {
            return walkMode;
        }

        public void SetWalkMode(WalkMode walkMode)
        {
            this.walkMode = walkMode;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public int GetEndurance()
        {
            return endurance;
        }

        public void SetEndurance(int endurance)
        {
            this.endurance = endurance;
        }

        public Vector2Int GetPosition()
        {
            return new Vector2Int(X, Y);
        }

        public void OnBeforeSerialize()
        {
            rankPackage = new RankPackage
            {
                BaseType = rank.GetBaseRank().ToString(),
                PromotedTimes = rank.GetPromotions().PromotedTimes()
            };
        }

        public void OnAfterDeserialize()
        {
            rank = rankPackage.GetRank();
            for (int i = 0; i < rankPackage.PromotedTimes; i++)
            {
                rank.GetPromotions().Promote();
            }

            instance = this;
        }
    }
}