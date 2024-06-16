using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared.Grid
{
    public class PathNodeComparer : IComparer<PathNode>
    {
        public int Compare(PathNode a, PathNode b)
        {
            return Mathf.Max(Mathf.Abs(a.X - b.X), Mathf.Abs(a.Y - b.Y));
        }
    }

    public class PathNode
    {
        private TileGrid<PathNode> grid;
        private int x;
        private int y;

        private int gCost;
        private int hCost;
        private int fCost;
        private bool isWalkable;
        private PathNode prevNode;

        public PathNode(TileGrid<PathNode> grid, int x, int y)
        {
            this.grid = grid;
            this.X = x;
            this.Y = y;
            this.IsWalkable = true;
        }

        public int GCost { get => gCost; set => gCost = value; }
        public int HCost { get => hCost; set => hCost = value; }
        public int FCost { get => fCost; }

        public PathNode PrevNode { get => prevNode; set => prevNode = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public bool IsWalkable { get => isWalkable; set => isWalkable = value; }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        public override string ToString()
        {
            return X + "," + Y;
        }
    }
}