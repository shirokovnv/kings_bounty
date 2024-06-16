using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared.Grid
{
    public class PathFinder
    {
        private const int MOVE_STRAIGTH_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        private TileGrid<PathNode> grid;
        private List<PathNode> openedList;
        private List<PathNode> closedList;

        public PathFinder(int width, int height)
        {
            grid = new TileGrid<PathNode>(
                width,
                height,
                (g, x, y) => new PathNode(g, x, y));
        }

        public List<PathNode> FindPath(Vector2Int start, Vector2Int end)
        {
            if (outOfBounds(start, end)) return null;

            PathNode startNode = grid.GetValue(start.x, start.y);
            PathNode endNode = grid.GetValue(end.x, end.y);

            if (!(startNode.IsWalkable && endNode.IsWalkable)) return null;

            openedList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();

            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    PathNode pathNode = grid.GetValue(x, y);
                    pathNode.GCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.PrevNode = null;
                }
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openedList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openedList);
                if (currentNode == endNode)
                {
                    // Final node reached
                    return CalculatePath(endNode);
                }

                openedList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighbour in GetNeighbours(currentNode))
                {
                    if (closedList.Contains(neighbour)) continue;
                    if (!neighbour.IsWalkable)
                    {
                        closedList.Add(neighbour);
                        continue;
                    }

                    int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbour);
                    if (tentativeGCost < neighbour.GCost)
                    {
                        neighbour.PrevNode = currentNode;
                        neighbour.GCost = tentativeGCost;
                        neighbour.HCost = CalculateDistanceCost(neighbour, endNode);
                        neighbour.CalculateFCost();

                        if (!openedList.Contains(neighbour))
                        {
                            openedList.Add(neighbour);
                        }
                    }
                }
            }

            // Final node is unreachable
            return null;
        }

        public TileGrid<PathNode> GetGrid() { return grid; }

        private bool outOfBounds(Vector2Int start, Vector2Int end)
        {
            return !(
                start.x >= 0 && start.x < grid.GetWidth() &&
                start.y >= 0 && start.y < grid.GetHeight() &&
                end.x >= 0 && end.x < grid.GetWidth() &&
                end.y >= 0 && end.y < grid.GetHeight()
            );
        }

        private List<PathNode> GetNeighbours(PathNode currentNode)
        {
            List<PathNode> neighbours = new List<PathNode>();

            int minX = Mathf.Max(currentNode.X - 1, 0);
            int maxX = Mathf.Min(currentNode.X + 1, grid.GetWidth() - 1);

            int minY = Mathf.Max(currentNode.Y - 1, 0);
            int maxY = Mathf.Min(currentNode.Y + 1, grid.GetHeight() - 1);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    PathNode node = grid.GetValue(x, y);
                    if (node != currentNode && node.IsWalkable)
                    {
                        neighbours.Add(node);
                    }
                }
            }

            return neighbours;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode> { endNode };
            PathNode currentNode = endNode;
            while (currentNode.PrevNode != null)
            {
                path.Add(currentNode.PrevNode);
                currentNode = currentNode.PrevNode;
            }

            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.X - b.X);
            int yDistance = Mathf.Abs(a.Y - b.Y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGTH_COST * remaining;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];
            for (int i = 1; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].FCost < lowestFCostNode.FCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }

            return lowestFCostNode;
        }
    }
}