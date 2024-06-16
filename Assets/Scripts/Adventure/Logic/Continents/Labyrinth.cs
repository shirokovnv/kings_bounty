using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents
{
    public enum LabyrinthTile
    {
        floor,
        wall
    }

    public class Labyrinth
    {
        private TileGrid<LabyrinthTile> grid;
        private TileGrid<int> segmentationMap;

        private readonly int width;
        private readonly int height;

        public Labyrinth(int width, int height)
        {
            this.width = width;
            this.height = height;

            grid = new TileGrid<LabyrinthTile>(
                width,
                height,
                (TileGrid<LabyrinthTile> g, int x, int y) => LabyrinthTile.floor
                );

            segmentationMap = new TileGrid<int>(
                width,
                height,
                (TileGrid<int> g, int x, int y) => 0
                );
        }

        public LabyrinthTile GetXY(int x, int y)
        {
            return grid.GetValue(x, y);
        }

        public void SetXY(int x, int y, LabyrinthTile tile)
        {
            grid.SetValue(x, y, tile);
        }

        public int GetSegmentXY(int x, int y)
        {
            return segmentationMap.GetValue(x, y);
        }

        public int GetWidth() { return width; }

        public int GetHeight() { return height; }

        public void Init()
        {
            int x = Random.Range(0, width / 2) * 2 + 1;
            int y = Random.Range(0, height / 2) * 2 + 1;

            List<List<int>> frontiers = new List<List<int>>();
            frontiers.Add(new List<int> { x, y, x, y });

            while (frontiers.Count > 0)
            {
                int index = Random.Range(0, frontiers.Count);
                List<int> list = frontiers[index];
                frontiers.RemoveAt(index);

                x = list[2];
                y = list[3];

                if (grid.GetValue(x, y) == LabyrinthTile.floor)
                {
                    grid.SetValue(x, y, LabyrinthTile.wall);
                    grid.SetValue(list[0], list[1], LabyrinthTile.wall);

                    if (x >= 2 && grid.GetValue(x - 2, y) == LabyrinthTile.floor)
                    {
                        frontiers.Add(new List<int> { x - 1, y, x - 2, y });
                    }
                    if (y >= 2 && grid.GetValue(x, y - 2) == LabyrinthTile.floor)
                    {
                        frontiers.Add(new List<int> { x, y - 1, x, y - 2 });
                    }
                    if (x < width - 2 && grid.GetValue(x + 2, y) == LabyrinthTile.floor)
                    {
                        frontiers.Add(new List<int> { x + 1, y, x + 2, y });
                    }
                    if (y < height - 2 && grid.GetValue(x, y + 2) == LabyrinthTile.floor)
                    {
                        frontiers.Add(new List<int> { x, y + 1, x, y + 2 });
                    }
                }
            }
        }

        public void InitSubdivision()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid.SetValue(x, y, LabyrinthTile.wall);
                }
            }

            RecursiveSubdivision(5, 5);
        }

        public void RecursiveSubdivision(int x, int y)
        {
            List<int> directions = new List<int> { 1, 2, 3, 4 };

            grid.SetValue(x, y, LabyrinthTile.floor);

            while (directions.Count > 0)
            {
                int index = Random.Range(0, directions.Count);
                int direction = directions.ElementAt(index);

                switch (direction)
                {
                    case 1: // up
                        if (y + 3 < height && grid.GetValue(x, y + 3) == LabyrinthTile.wall)
                        {
                            grid.SetValue(x, y + 1, LabyrinthTile.floor);
                            grid.SetValue(x, y + 2, LabyrinthTile.floor);
                            RecursiveSubdivision(x, y + 3);
                        }
                        break;
                    case 2: // down
                        if (y - 3 >= 0 && grid.GetValue(x, y - 3) == LabyrinthTile.wall)
                        {
                            grid.SetValue(x, y - 1, LabyrinthTile.floor);
                            grid.SetValue(x, y - 2, LabyrinthTile.floor);
                            RecursiveSubdivision(x, y - 3);
                        }
                        break;
                    case 3: // left
                        if (x - 3 >= 0 && grid.GetValue(x - 3, y) == LabyrinthTile.wall)
                        {
                            grid.SetValue(x - 1, y, LabyrinthTile.floor);
                            grid.SetValue(x - 2, y, LabyrinthTile.floor);
                            RecursiveSubdivision(x - 3, y);
                        }
                        break;
                    case 4: // right
                        if (x + 3 < width && grid.GetValue(x + 3, y) == LabyrinthTile.wall)
                        {
                            grid.SetValue(x + 1, y, LabyrinthTile.floor);
                            grid.SetValue(x + 2, y, LabyrinthTile.floor);
                            RecursiveSubdivision(x + 3, y);
                        }
                        break;
                }

                directions.RemoveAt(index);
            }
        }

        public void InitSegmentation(float subSegmentRatio = 0.25f, int numSubSegments = 3)
        {
            int size = width * height;

            float subSquare = size * subSegmentRatio / numSubSegments;

            int subSide = Mathf.FloorToInt(Mathf.Sqrt(subSquare));
            int halfSubSide = subSide / 2;

            List<Vector2Int> quads = new List<Vector2Int>();

            bool flag = true;
            int iterations = 1000;

            while (flag)
            {
                int x = Random.Range(halfSubSide, width - halfSubSide);
                int y = Random.Range(halfSubSide, height - halfSubSide);

                var intersectionQuads = quads.Where(quad =>
                {
                    return Mathf.Max(Mathf.Abs(quad.x - x), Mathf.Abs(quad.y - y)) <= subSide + 1;
                });

                if (intersectionQuads.Count() == 0)
                {
                    quads.Add(new Vector2Int(x, y));
                    Segmentation(x, y, halfSubSide, 1);
                    numSubSegments--;
                }

                if (numSubSegments == 0 || iterations <= 0)
                {
                    flag = false;
                }

                iterations--;
            }
        }

        private void Segmentation(int x, int y, int size, int segment)
        {
            for (int i = x - size; i <= x + size; i++)
            {
                for (int j = y - size; j <= y + size; j++)
                {
                    segmentationMap.SetValue(i, j, segment);
                }
            }
        }

        public void RemoveDeadEnds(int numIterations)
        {
            for (int i = 0; i < numIterations; i++)
            {
                List<Vector2Int> deadEnds = new List<Vector2Int>();

                for (int w = 0; w < width; w++)
                {
                    for (int h = 0; h < height; h++)
                    {
                        if (grid.GetValue(w, h) == LabyrinthTile.floor)
                        {
                            int neighbours = 0;

                            if (h - 1 >= 0 && grid.GetValue(w, h - 1) == LabyrinthTile.floor)
                            {
                                neighbours++;
                            }

                            if (h + 1 < height && grid.GetValue(w, h + 1) == LabyrinthTile.floor)
                            {
                                neighbours++;
                            }

                            if (w - 1 >= 0 && grid.GetValue(w - 1, h) == LabyrinthTile.floor)
                            {
                                neighbours++;
                            }

                            if (w + 1 < width && grid.GetValue(w - 1, h) == LabyrinthTile.floor)
                            {
                                neighbours++;
                            }

                            if (neighbours <= 1)
                            {
                                deadEnds.Add(new Vector2Int(w, h));
                            }
                        }
                    }
                }

                deadEnds.ForEach(deadEnd => grid.SetValue(deadEnd.x, deadEnd.y, LabyrinthTile.wall));
            }
        }

        public void CellularAutomate(int numIterations)
        {
            for (int i = 0; i < numIterations; i++)
            {
                List<Vector2Int> newCells = new List<Vector2Int>();

                for (int w = 0; w < width; w++)
                {
                    for (int h = 0; h < height; h++)
                    {
                        if (grid.GetValue(w, h) == LabyrinthTile.wall)
                        {
                            int neighbours = 0;

                            for (int a = 0; a < 3; a++)
                            {
                                for (int b = 0; b < 3; b++)
                                {
                                    int neighbourX = w - a;
                                    int neighbourY = h - b;

                                    if (
                                        neighbourX >= 0 &&
                                        neighbourY >= 0 &&
                                        neighbourX < width &&
                                        neighbourY < height &&
                                        grid.GetValue(neighbourX, neighbourY) == LabyrinthTile.floor
                                        )
                                    {
                                        neighbours++;
                                    }
                                }
                            }

                            if (neighbours >= 4)
                            {
                                newCells.Add(new Vector2Int(w, h));
                            }
                        }

                    }
                }

                newCells.ForEach(cell => grid.SetValue(cell.x, cell.y, LabyrinthTile.floor));
            }
        }
    }

}
