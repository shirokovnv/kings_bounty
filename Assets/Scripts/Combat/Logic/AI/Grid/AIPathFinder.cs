using Assets.Scripts.Combat.Logic.AI.Decisions;
using Assets.Scripts.Shared.Grid;
using Assets.Scripts.Shared.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Combat.Logic.AI.Grid
{
    public static class AIPathFinder
    {
        // Find paths to all enemies
        public static List<List<PathNode>> FindPathsToEnemies(DecisionContext context)
        {
            List<List<PathNode>> paths = new();

            if (context.OGroups.Count <= 0)
            {
                return paths;
            }

            PathFinder pathFinder = new(context.GridWidth, context.GridHeight);
            TileGrid<PathNode> grid = pathFinder.GetGrid();

            // set obstacles
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    grid.GetValue(x, y).IsWalkable = !context.Grid.GetValue(x, y).IsObstacle();
                }
            }

            var currentUnit = context.PGroup;

            context.PGroups.ForEach(group =>
            {
                // Set walkable = true for current unit, and false for the others
                grid.GetValue(group.X, group.Y).IsWalkable =
                    group.X == currentUnit.X && group.Y == currentUnit.Y;
            });

            context.OGroups.ForEach(group =>
            {
                grid.GetValue(group.X, group.Y).IsWalkable = false;
            });

            for (int enemyIndex = 0; enemyIndex < context.OGroups.Count; enemyIndex++)
            {
                var currentEnemy = context.OGroups[enemyIndex];

                grid.GetValue(currentEnemy.X, currentEnemy.Y).IsWalkable = true;

                Vector2Int start = new(currentUnit.X, currentUnit.Y);
                Vector2Int end = new(currentEnemy.X, currentEnemy.Y);
                var path = pathFinder.FindPath(start, end);
                if (path != null)
                {
                    paths.Add(path);
                }

                grid.GetValue(currentEnemy.X, currentEnemy.Y).IsWalkable = false;
            }

            return paths;
        }

        // find path from current unit to target position
        public static List<PathNode> FindPathToPosition(DecisionContext context, int x0, int y0)
        {
            PathFinder pathFinder = new(context.GridWidth, context.GridHeight);
            TileGrid<PathNode> grid = pathFinder.GetGrid();

            // set obstacles
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    grid.GetValue(x, y).IsWalkable = !context.Grid.GetValue(x, y).IsObstacle();
                }
            }

            var currentUnit = context.PGroup;

            context.PGroups.ForEach(group =>
            {
                grid.GetValue(group.X, group.Y).IsWalkable =
                                group.X == currentUnit.X && group.Y == currentUnit.Y;
            });

            context.OGroups.ForEach(group =>
            {
                grid.GetValue(group.X, group.Y).IsWalkable = false;
            });

            var start = new Vector2Int(context.PGroup.X, context.PGroup.Y);
            var end = new Vector2Int(x0, y0);

            var path = pathFinder.FindPath(start, end);

            return path ?? new List<PathNode>();
        }

        // find all paths even through obstacles (for flyers) to all enemies
        public static List<List<PathNode>> FindFlyingPathsToEnemies(DecisionContext context)
        {
            List<List<PathNode>> paths = new();

            if (context.OGroups.Count <= 0)
            {
                return paths;
            }

            foreach (var group in context.OGroups)
            {
                var path = FindFlyingPathToTarget(context, group);

                if (path.Count > 0)
                {
                    paths.Add(path);
                }
            }

            return paths;
        }

        // find path for flyer to the single enemy
        public static List<PathNode> FindFlyingPathToTarget(DecisionContext data, UnitGroup oUnit)
        {
            var pUnit = data.PGroup;

            List<PathNode> path = new();

            TileGrid<PathNode> grid = new(
                data.GridWidth,
                data.GridHeight,
                (g, x, y) => new PathNode(g, x, y)
                );

            // set obstacles
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    grid.GetValue(x, y).IsWalkable = !data.Grid.GetValue(x, y).IsObstacle();
                }
            }

            if (Utils.ManDistance(pUnit.X, pUnit.Y, oUnit.X, oUnit.Y) == 1)
            {
                path.Add(new PathNode(grid, pUnit.X, pUnit.Y));
                path.Add(new PathNode(grid, oUnit.X, oUnit.Y));

                return path;
            }

            data.PGroups.ForEach(group =>
            {
                // Set walkable = true for current unit, and false for the others
                grid.GetValue(group.X, group.Y).IsWalkable =
                    group.X == pUnit.X && group.Y == pUnit.Y;
            });

            data.OGroups.ForEach(group =>
            {
                grid.GetValue(group.X, group.Y).IsWalkable = false;
            });

            grid.GetValue(oUnit.X, oUnit.Y).IsWalkable = true;

            int startX = Mathf.Max(0, oUnit.X - 1);
            int startY = Mathf.Max(0, oUnit.Y - 1);

            int endX = Mathf.Min(data.GridWidth - 1, oUnit.X + 1);
            int endY = Mathf.Min(data.GridHeight - 1, oUnit.Y + 1);

            List<PathNode> potentialNodes = new();
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    if (x != oUnit.X && y != oUnit.Y && grid.GetValue(x, y).IsWalkable)
                    {
                        potentialNodes.Add(grid.GetValue(x, y));
                    }
                }
            }

            if (potentialNodes.Count == 0)
            {
                return path;
            }

            potentialNodes.Sort(new PathNodeComparer());
            var closest = potentialNodes.First();

            path.Add(new PathNode(grid, pUnit.X, pUnit.Y));
            path.Add(closest);
            path.Add(new PathNode(grid, oUnit.X, oUnit.Y));

            return path;
        }

        // find path from blocked to free position (for ranged)
        public static List<PathNode> FindFallbackPathToPosition(DecisionContext context)
        {
            List<PathNode> neighbours = GetFreeCellsInMovementRange(context);

            if (neighbours.Count == 0)
            {
                return new List<PathNode>();
            }

            foreach (PathNode node in neighbours)
            {
                var path = FindPathToPosition(context, node.X, node.Y);
                if (path.Count > 0 && path.Count <= context.PGroup.CurrentMoves)
                {
                    return path;
                }
            }

            return new List<PathNode>();
        }

        // get list of nodes, potentially free in current movement area
        public static List<PathNode> GetFreeCellsInMovementRange(DecisionContext data)
        {
            UnitGroup pUnit = data.PGroup;

            int startX = Mathf.Max(0, pUnit.X - pUnit.CurrentMoves + 1);
            int startY = Mathf.Max(0, pUnit.Y - pUnit.CurrentMoves + 1);

            int endX = Mathf.Min(data.GridWidth - 1, pUnit.X + pUnit.CurrentMoves - 1);
            int endY = Mathf.Min(data.GridHeight - 1, pUnit.Y + pUnit.CurrentMoves - 1);

            // generate grid
            TileGrid<PathNode> grid = new(
                data.GridWidth,
                data.GridHeight,
                (g, x, y) => new PathNode(g, x, y)
                );

            // set obstacles
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    grid.GetValue(x, y).IsWalkable = !data.Grid.GetValue(x, y).IsObstacle();
                }
            }

            data.PGroups.ForEach(group =>
            {
                grid.GetValue(group.X, group.Y).IsWalkable = false;
            });

            data.OGroups.ForEach(group =>
            {
                grid.GetValue(group.X, group.Y).IsWalkable = false;
            });

            List<PathNode> neighbours = new();

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    if (grid.GetValue(x, y).IsWalkable && !IsCellBlocked(x, y, data.OGroups))
                    {
                        neighbours.Add(grid.GetValue(x, y));
                    }
                }
            }

            return neighbours;
        }

        // check cell is blocked
        public static bool IsCellBlocked(int x, int y, List<UnitGroup> groups)
        {
            return groups.Where((group) => Utils.ManDistance(x, y, group.X, group.Y) == 1).Count() > 0;
        }
    }
}