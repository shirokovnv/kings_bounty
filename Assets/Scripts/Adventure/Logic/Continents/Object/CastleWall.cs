using Assets.Scripts.Adventure.Logic.Continents.Base;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Object
{
    [System.Serializable]
    public class CastleWall : BaseObject
    {
        [SerializeField] private int tileIndex;

        private CastleWall(string name) : base(name, ObjectType.castleWall)
        {
        }

        public int GetTileIndex()
        {
            return tileIndex;
        }

        public void SetTileIndex(int value)
        {
            tileIndex = value;
        }

        public static CastleWall Create(int x, int y, int continentNumber, int tileIndex)
        {
            return new CastleWall($"castleWall({x}{y}{continentNumber})")
            {
                X = x,
                Y = y,
                ContinentNumber = continentNumber,

                tileIndex = tileIndex
            };
        }
    }
}