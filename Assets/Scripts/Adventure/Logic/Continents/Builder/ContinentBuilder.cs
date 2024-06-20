using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Continents.Config;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.Logic.Continents.Object.Biome;
using Assets.Scripts.Shared.Data;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Grid;
using Assets.Scripts.Shared.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Builder
{
    public class ContinentBuilder : IContinentBuilder
    {
        private const int MAX_ITERATIONS_COUNT = 1000;
        private const float MIN_ERROR = 0.0f;
        private const float MAX_ERROR = 0.5f;

        private TileGrid<ContinentTile> grid;
        private Labyrinth labyrinth;
        private readonly ContinentConfig config;

        private readonly List<UnitScriptableObject> units;

        private List<City> cities;
        private List<Castle> castles;

        public enum PatchMode
        {
            LeftBottom,
            RightBottom,
            LeftTop,
            RightTop,
        }

        public ContinentBuilder(
            ContinentConfig config,
            List<UnitScriptableObject> units
            )
        {
            this.units = units;
            this.config = config;
        }

        public TileGrid<ContinentTile> Build()
        {
            PatchInconsistentTiles();
            SetSibingsForCities();

            return grid;
        }

        public IContinentBuilder WithInitialState()
        {
            cities = new List<City>();
            castles = new List<Castle>();

            grid = new TileGrid<ContinentTile>(
                config.GetWidth(),
                config.GetHeight(),
                (g, x, y) => new ContinentTile(
                    new BiomeObject("", BiomeType.none),
                    null
                    )
                );

            labyrinth = new Labyrinth(config.GetWidth(), config.GetHeight());

            return this;
        }

        public IContinentBuilder WithBase(BiomeType biomeType)
        {
            for (int x = config.GetWaterEdge(); x < config.GetWidth() - config.GetWaterEdge(); x++)
            {
                for (int y = config.GetWaterEdge(); y < config.GetHeight() - config.GetWaterEdge(); y++)
                {
                    grid.GetValue(x, y).SetBiome(biomeType);
                }
            }

            return this;
        }

        public IContinentBuilder WithWaterEdges()
        {
            for (int w = 0; w < config.GetWaterEdge(); w++)
            {
                for (int y = 0; y < config.GetHeight(); y++)
                {
                    grid.GetValue(w, y).SetBiome(BiomeType.water);
                    grid.GetValue(w + config.GetWidth() - config.GetWaterEdge(), y).SetBiome(BiomeType.water);
                }
            }

            for (int w = 0; w < config.GetWaterEdge(); w++)
            {
                for (int x = 0; x < config.GetWidth(); x++)
                {
                    grid.GetValue(x, w).SetBiome(BiomeType.water);
                    grid.GetValue(x, w + config.GetHeight() - config.GetWaterEdge()).SetBiome(BiomeType.water);
                }
            }

            return this;
        }

        public IContinentBuilder WithSandAreas()
        {
            if (config.GetBiomeConfig().GetSandLength() <= 0) return this;

            Vector2Int startOfTheSand = RandomPointFor2x2Patch(BiomeType.grass);

            RecursiveBuildAreas(
                startOfTheSand,
                config.GetBiomeConfig().GetSandLength(),
                config.GetBiomeConfig().GetSandContinuity(),
                BiomeType.desert,
                true
                );

            return this;
        }

        public IContinentBuilder WithRiversAndLakes()
        {
            if (config.GetBiomeConfig().GetWaterLength() <= 0) return this;

            Vector2Int startOfTheRiver = RandomPointFor2x2Patch();

            RecursiveBuildAreas(
                startOfTheRiver,
                config.GetBiomeConfig().GetWaterLength(),
                config.GetBiomeConfig().GetWaterContinuity(),
                BiomeType.water
                );

            return this;
        }

        public IContinentBuilder WithIslands(int numIslands, int minRadius, int maxRadius)
        {
            if (minRadius <= 0 || maxRadius <= 0)
            {
                return this;
            }

            if (minRadius > maxRadius)
            {
                (minRadius, maxRadius) = (maxRadius, minRadius);
            }

            Dictionary<Vector2Int, int> circles = new();

            int numIterations = 0;

            while (circles.Count < numIslands && numIterations < MAX_ITERATIONS_COUNT)
            {
                RandomPointWithinShores(out int x, out int y);

                int radius = Random.Range(minRadius, maxRadius + 1);

                bool isValidCenter = circles
                    .Where(c => Utils.EuclidDistanceSquared(x, y, c.Key.x, c.Key.y) < 4 * (radius + 1) * (c.Value + 1))
                    .Count() == 0;

                if (isValidCenter)
                {
                    circles.Add(new Vector2Int(x, y), radius);
                }

                numIterations++;
            }

            List<int> choises = new() { -1, 0, 1 };

            foreach (var (center, radius) in circles)
            {
                int maxLength = 4 * radius * radius;

                int x = center.x;
                int y = center.y;

                float error = Random.Range(MIN_ERROR, MAX_ERROR);

                int islandLength = Mathf.FloorToInt(maxLength * (1.0f - error));

                for (int l = 0; l < islandLength; l++)
                {
                    grid.GetValue(x, y).SetBiome(BiomeType.grass);

                    numIterations = 0;
                    while (numIterations <= MAX_ITERATIONS_COUNT)
                    {
                        var (dx, dy) = (
                                choises[Random.Range(0, choises.Count)],
                                choises[Random.Range(0, choises.Count)]
                            );

                        int newX = x + dx;
                        int newY = y + dy;

                        if (
                            IsWithinShores(newX, newY) &&
                            Utils.EuclidDistanceSquared(newX, newY, center.x, center.y) <= radius * radius &&
                            x != newX &&
                            y != newY)
                        {
                            x = newX;
                            y = newY;

                            numIterations += MAX_ITERATIONS_COUNT;
                        }

                        numIterations++;
                    }

                }
            }

            PatchInconsistentTiles();

            return this;
        }

        public IContinentBuilder WithMainCastleAndCity()
        {
            if (config.GetNumber() != 1)
            {
                return this;
            }

            PlaceMainCastle();
            PlaceMainCity();

            return this;
        }

        public IContinentBuilder WithCastles()
        {
            int numCastles = 0;
            int numIterations = 0;

            while (numCastles < config.GetObjectConfig().GetCastlesCount() && numIterations < MAX_ITERATIONS_COUNT)
            {
                RandomPointWithinShores(out int x, out int y);

                Vector2Int position = new(x, y);

                ContinentTile xy00 = grid.GetValue(x, y);
                ContinentTile xy01 = grid.GetValue(x - 1, y);
                ContinentTile xy02 = grid.GetValue(x + 1, y);

                // Bottom cell contains grass
                ContinentTile xy03 = grid.GetValue(x, y - 1);

                // Top cells contains grass
                ContinentTile xy04 = grid.GetValue(x - 1, y + 1);
                ContinentTile xy05 = grid.GetValue(x, y + 1);
                ContinentTile xy06 = grid.GetValue(x + 1, y + 1);

                if (xy00.IsEmptyGround() &&
                    xy01.IsEmptyGround() &&
                    xy02.IsEmptyGround() &&
                    xy03.IsEmptyGround() &&

                    xy04.IsEmptyGround() &&
                    xy05.IsEmptyGround() &&
                    xy06.IsEmptyGround() &&
                    IsObjectNearby(ObjectType.castleGate, position, 5) == false)
                {
                    int strength = Random.Range(
                        config.GetObjectConfig().GetMinCaptainStrength() + config.GetObjectConfig().GetCastleStrengthBonus(),
                        config.GetObjectConfig().GetMaxCaptainStrength() + config.GetObjectConfig().GetCastleStrengthBonus() + 1
                        );
                    var castleGate = Castle.CreateRandom(x, y, config.GetNumber(), units, strength);
                    castleGate.SetOwner(CastleOwner.opponent);

                    grid.GetValue(x - 1, y + 1).ObjectLayer = CastleWall.Create(x - 1, y + 1, config.GetNumber(), 0);
                    grid.GetValue(x, y + 1).ObjectLayer = CastleWall.Create(x, y + 1, config.GetNumber(), 1);
                    grid.GetValue(x + 1, y + 1).ObjectLayer = CastleWall.Create(x + 1, y + 1, config.GetNumber(), 2);

                    grid.GetValue(x - 1, y).ObjectLayer = CastleWall.Create(x - 1, y, config.GetNumber(), 3);
                    grid.GetValue(x + 1, y).ObjectLayer = CastleWall.Create(x + 1, y, config.GetNumber(), 5);
                    grid.GetValue(x, y).ObjectLayer = castleGate;

                    castles.Add(castleGate);

                    numCastles++;
                }

                numIterations++;
            }

            return this;
        }

        public IContinentBuilder WithCities()
        {
            int numCities = 0;
            int numIterations = 0;

            var shorePoints = GetShorePoints().OrderBy(point => Random.value).ToList();

            while (
                numCities < config.GetObjectConfig().GetCitiesCount() &&
                shorePoints.Count > 0 &&
                numIterations < MAX_ITERATIONS_COUNT)
            {
                var newPoint = shorePoints.ElementAt(0);
                int x = newPoint.x;
                int y = newPoint.y;

                ContinentTile tile = grid.GetValue(x, y);
                Vector2Int position = new Vector2Int(x, y);

                if (tile.IsEmptyGround() &&
                    IsObjectNearby(ObjectType.castleGate, position, 2) == false &&
                    IsObjectNearby(ObjectType.city, position, 2) == false &&
                    IsBiomeNearby(BiomeType.water, position, 1, 3)
                    )
                {
                    var randomSpell = SpellManager.Instance().GetRandomSpell();

                    var city = City.Create(x, y, config.GetNumber(), randomSpell);

                    tile.ObjectLayer = city;
                    cities.Add(city);

                    numCities++;
                }

                shorePoints.RemoveAt(0);

                numIterations++;
            }

            return this;
        }

        public IContinentBuilder WithChests()
        {
            var chests = new List<Chest>();

            int bounds = config.GetWaterEdge();

            if (config.GetSandRingEdge() > 0)
            {
                bounds += 1;
            }

            for (int x = bounds; x < config.GetWidth() - bounds; x++)
            {
                for (int y = bounds; y < config.GetHeight() - bounds; y++)
                {
                    ContinentTile tile = grid.GetValue(x, y);
                    Vector2Int position = new(x, y);

                    if (tile.IsEmptyGround() &&
                        IsObjectNearby(ObjectType.castleWall, position, 1) == false &&
                        Random.value < config.GetObjectConfig().GetChestFrequency()
                        )
                    {
                        var chestValue = Random.Range(
                            config.GetObjectConfig().GetMinChestStrength(),
                            config.GetObjectConfig().GetMaxChestStrength() + 1
                            );

                        var chest = Chest.CreateRandom(x, y, config.GetNumber(), chestValue);
                        tile.ObjectLayer = chest;

                        chests.Add(chest);
                    }
                }
            }

            // place one map and one map reveal
            var types = config.GetNumber() < config.GetMaxContinentsCount()
                ? new[] { Chest.ChestType.MapReveal, Chest.ChestType.PathToContinent }
                : new[] { Chest.ChestType.MapReveal };

            if (chests.Count < types.Length)
            {
                return this;
            }

            chests = chests.OrderBy(_ => Random.value).ToList();
            for (int i = 0; i < types.Length; i++)
            {
                chests.ElementAt(i).SetChestType(types[i]);
            }

            chests.Clear();

            return this;
        }

        public IContinentBuilder WithDwellings()
        {
            int randomDwellingsCount = Random.Range(
                        config.GetObjectConfig().GetMinDwellingsCount(),
                        config.GetObjectConfig().GetMaxDwellingsCount() + 1);

            int numDwellings = 0;
            int numIterations = 0;

            while (numDwellings < randomDwellingsCount && numIterations < MAX_ITERATIONS_COUNT)
            {
                int additionalBounds = config.GetSandRingEdge() > 0
                    ? 1
                    : 0;

                RandomPointWithinShores(out int x, out int y, additionalBounds);

                ContinentTile tile = grid.GetValue(x, y);
                Vector2Int position = new Vector2Int(x, y);

                if (tile.IsEmptyGround() &&
                    IsObjectNearby(ObjectType.castleGate, position, 2) == false &&
                    IsObjectNearby(ObjectType.city, position, 2) == false
                    )
                {
                    tile.ObjectLayer = Dwelling.Initialize(
                        x,
                        y,
                        config.GetNumber(),
                        config.GetObjectConfig().GetMinDwellingCreatureLevel(),
                        config.GetObjectConfig().GetMaxDwellingCreatureLevel()
                        );
                    numDwellings++;
                }

                numIterations++;
            }

            return this;
        }

        public IContinentBuilder WithArtifacts()
        {
            var artifacts = ArtifactManager.Instance().GetArtifactsByLevel(config.GetNumber());

            int maxArtifacts = artifacts.Count;
            int numArtifacts = 0;
            int numIterations = 0;

            while (numArtifacts < maxArtifacts && numIterations < MAX_ITERATIONS_COUNT)
            {
                int additionalBounds = config.GetSandRingEdge() > 0
                    ? 1
                    : 0;

                RandomPointWithinShores(out int x, out int y, additionalBounds);

                ContinentTile tile = grid.GetValue(x, y);
                Vector2Int position = new Vector2Int(x, y);

                if (tile.IsEmptyGround() &&
                    IsObjectNearby(ObjectType.castleWall, position, 1) == false
                    )
                {
                    var artifact = artifacts.ElementAt(0);

                    tile.ObjectLayer = new Artifact(x, y, config.GetNumber(), artifact);
                    numArtifacts++;

                    artifacts.RemoveAt(0);
                }

                numIterations++;
            }

            return this;
        }

        public IContinentBuilder WithCaptains()
        {
            int randomCaptainsCount = Random.Range(config.GetObjectConfig().GetMinCaptainsCount(), config.GetObjectConfig().GetMaxCaptainsCount() + 1);
            int numCaptains = 0;
            int numIterations = 0;

            while (numCaptains < randomCaptainsCount && numIterations < MAX_ITERATIONS_COUNT)
            {
                RandomPointWithinShores(out int x, out int y);

                ContinentTile tile = grid.GetValue(x, y);
                ContinentTile topTile = grid.GetValue(x, y + 1);

                if (tile.IsEmptyGround() &&
                    topTile.HasObject(ObjectType.castleGate) == false
                    )
                {
                    int strength = Random.Range(
                        config.GetObjectConfig().GetMinCaptainStrength(),
                        config.GetObjectConfig().GetMaxCaptainStrength() + 1);

                    int numGroups = Random.Range(
                        config.GetObjectConfig().GetMinCaptainsGroups(),
                        config.GetObjectConfig().GetMaxCaptainsGroups() + 1);

                    var captain = Captain.CreateRandom(x, y, config.GetNumber(), units, numGroups, strength);

                    tile.ObjectLayer = captain;
                    numCaptains++;
                }

                numIterations++;
            }

            return this;
        }

        public IContinentBuilder WithSigns()
        {
            for (int x = config.GetWaterEdge(); x < config.GetWidth() - config.GetWaterEdge(); x++)
            {
                for (int y = config.GetWaterEdge(); y < config.GetHeight() - config.GetWaterEdge(); y++)
                {
                    ContinentTile tile = grid.GetValue(x, y);
                    Vector2Int position = new Vector2Int(x, y);

                    if (tile.IsEmptyGround() &&
                        IsObjectNearby(ObjectType.castleWall, position, 1) == false &&
                        IsObjectNearby(ObjectType.sign, position, 3) == false &&
                        Random.value < config.GetObjectConfig().GetSignFrequency()
                        )
                    {
                        var sign = Sign.Create(x, y, config.GetNumber());
                        sign.SetTitle(NamingData.Instance().GetRandomSignTitle());

                        tile.ObjectLayer = sign;
                    }
                }
            }

            return this;
        }

        public IContinentBuilder WithBiomes()
        {
            labyrinth.InitSubdivision();

            int numSegments = Random.Range(1, 4);

            labyrinth.InitSegmentation(config.GetBiomeConfig().GetSubBiome().Value, numSegments);

            int bounds = config.GetWaterEdge();

            if (config.GetSandRingEdge() > 0)
            {
                BuildSandRing();
                bounds += config.GetSandRingEdge();
            }

            var mainBiome = config.GetBiomeConfig().GetMainBiome().Key;
            var subBiome = config.GetBiomeConfig().GetSubBiome().Key;

            for (int x = bounds; x < config.GetWidth() - bounds; x++)
            {
                for (int y = bounds; y < config.GetHeight() - bounds; y++)
                {
                    ContinentTile tile = grid.GetValue(x, y);
                    ContinentTile topTile = grid.GetValue(x, y + 1);
                    LabyrinthTile labyrinthTile = labyrinth.GetXY(x, y);
                    int segment = labyrinth.GetSegmentXY(x, y);

                    if (tile.IsEmptyGround() &&
                        topTile.HasObject(ObjectType.castleGate) == false &&
                        labyrinthTile == LabyrinthTile.wall
                        )
                    {
                        var biome = segment == 0 ? mainBiome : subBiome;
                        tile.SetBiome(biome);
                    }
                }
            }

            return this;
        }

        private void PatchInconsistentTiles()
        {
            int numIterations = 5;

            int numPatchedTiles = -1;

            while (numPatchedTiles != 0 && numIterations > 0)
            {
                ConfigureBiomeTiles();
                numPatchedTiles = ClearInconsistentTiles();

                numIterations--;
            }
        }

        private void ConfigureBiomeTiles()
        {
            int bounds = config.GetWaterEdge() - 1;

            for (int x = bounds; x < config.GetWidth() - bounds; x++)
            {
                for (int y = bounds; y < config.GetHeight() - bounds; y++)
                {
                    var currentBiome = grid.GetValue(x, y).BiomeLayer.GetBiomeType();
                    int index = 12;

                    if (currentBiome == BiomeType.grass)
                    {
                        index = 0;

                        if (grid.GetValue(x, y + 1).HasObject(ObjectType.castleGate))
                        {
                            index = 1;
                        }
                    }

                    if (currentBiome != BiomeType.grass)
                    {
                        var leftTop = grid.GetValue(x - 1, y + 1).BiomeLayer.GetBiomeType() == currentBiome;
                        var top = grid.GetValue(x, y + 1).BiomeLayer.GetBiomeType() == currentBiome;
                        var rightTop = grid.GetValue(x + 1, y + 1).BiomeLayer.GetBiomeType() == currentBiome;

                        var left = grid.GetValue(x - 1, y).BiomeLayer.GetBiomeType() == currentBiome;
                        var right = grid.GetValue(x + 1, y).BiomeLayer.GetBiomeType() == currentBiome;

                        var leftBottom = grid.GetValue(x - 1, y - 1).BiomeLayer.GetBiomeType() == currentBiome;
                        var bottom = grid.GetValue(x, y - 1).BiomeLayer.GetBiomeType() == currentBiome;
                        var rightBottom = grid.GetValue(x + 1, y - 1).BiomeLayer.GetBiomeType() == currentBiome;

                        if (left && right && top && !bottom)
                        {
                            index = 11;
                        }

                        if (left && right && bottom && !top)
                        {
                            index = 10;
                        }

                        if (top && bottom && right && !left)
                        {
                            index = 9;
                        }

                        if (top && bottom && left && !right)
                        {
                            index = 8;
                        }

                        if (left && top && !leftTop)
                        {
                            index = 7;
                        }

                        if (left && bottom && !leftBottom)
                        {
                            index = 6;
                        }

                        if (right && top && !rightTop)
                        {
                            index = 5;
                        }

                        if (right && bottom && !rightBottom)
                        {
                            index = 4;
                        }

                        if (top && left && !right && !bottom)
                        {
                            index = 3;
                        }

                        if (left && bottom && !right && !top)
                        {
                            index = 2;
                        }

                        if (top && right && !left && !bottom)
                        {
                            index = 1;
                        }

                        if (bottom && right && !left && !top)
                        {
                            index = 0;
                        }
                    }

                    grid.GetValue(x, y).BiomeLayer.SetTileIndex(index);
                }
            }
        }

        private void RecursiveBuildAreas(
            Vector2Int currentPoint,
            int length,
            float continuity,
            BiomeType biome,
            bool checkShores = false
        )
        {
            if (length <= 0) return;

            grid.GetValue(currentPoint.x, currentPoint.y).SetBiome(biome);

            Vector2Int nextPoint;
            List<Vector2Int> neighbours = FreeNeighbours(currentPoint, continuity);

            if (checkShores)
            {
                neighbours = neighbours.Where(n => IsWithinShores(n.x, n.y)).ToList();
            }

            if (neighbours.Count > 0)
            {
                foreach (var neighbour in neighbours)
                {
                    grid.GetValue(neighbour.x, neighbour.y).SetBiome(biome);

                    if (!checkShores || IsWithinShores(neighbour.x + 1, neighbour.y))
                    {
                        grid.GetValue(neighbour.x + 1, neighbour.y).SetBiome(biome);
                    }

                    if (!checkShores || IsWithinShores(neighbour.x + 1, neighbour.y + 1))
                    {
                        grid.GetValue(neighbour.x + 1, neighbour.y + 1).SetBiome(biome);
                    }

                    if (!checkShores || IsWithinShores(neighbour.x, neighbour.y + 1))
                    {
                        grid.GetValue(neighbour.x, neighbour.y + 1).SetBiome(biome);
                    }
                }

                int neighbourIndex = Random.Range(0, neighbours.Count);
                nextPoint = neighbours[neighbourIndex];
            }
            else
            {
                nextPoint = RandomPointFor2x2Patch();
            }

            RecursiveBuildAreas(nextPoint, length - 1, continuity, biome, checkShores);
        }

        private void BuildSandRing()
        {
            for (int w = 0; w < config.GetSandRingEdge(); w++)
            {
                for (int y = config.GetWaterEdge() + 1; y < config.GetHeight() - config.GetWaterEdge(); y++)
                {
                    Vector2Int p00 = new Vector2Int(w + config.GetWaterEdge() + 1, y);
                    Vector2Int p02 = new Vector2Int(config.GetWidth() - config.GetWaterEdge() - 2 - w, y);

                    if (grid.GetValue(p00.x, p00.y).IsEmptyGround() &&
                        IsObjectNearby(ObjectType.castleWall, p00, 1) == false &&
                        IsBiomeNearby(BiomeType.water, p00, 1) == false
                        )
                    {
                        grid.GetValue(p00.x, p00.y).SetBiome(BiomeType.desert);
                    }

                    if (grid.GetValue(p02.x, p02.y).IsEmptyGround() &&
                        IsObjectNearby(ObjectType.castleWall, p02, 1) == false &&
                        IsBiomeNearby(BiomeType.water, p02, 1) == false
                        )
                    {
                        grid.GetValue(p02.x, p02.y).SetBiome(BiomeType.desert);
                    }
                }
            }

            for (int w = 0; w < config.GetSandRingEdge(); w++)
            {
                for (int x = config.GetWaterEdge() + 1; x < config.GetWidth() - config.GetWaterEdge(); x++)
                {
                    Vector2Int p01 = new Vector2Int(x, w + config.GetWaterEdge() + 1);
                    Vector2Int p03 = new Vector2Int(x, config.GetHeight() - config.GetWaterEdge() - 2 - w);

                    if (grid.GetValue(p01.x, p01.y).IsEmptyGround() &&
                        IsObjectNearby(ObjectType.castleWall, p01, 1) == false &&
                        IsBiomeNearby(BiomeType.water, p01, 1) == false
                        )
                    {
                        grid.GetValue(p01.x, p01.y).SetBiome(BiomeType.desert);
                    }

                    if (grid.GetValue(p03.x, p03.y).IsEmptyGround() &&
                        IsObjectNearby(ObjectType.castleWall, p03, 1) == false &&
                        IsBiomeNearby(BiomeType.water, p03, 1) == false
                        )
                    {
                        grid.GetValue(p03.x, p03.y).SetBiome(BiomeType.desert);
                    }
                }
            }
        }

        private void SetSibingsForCities()
        {
            cities.ForEach(city =>
            {
                city.SetWaterSiblings(GetSiblings(city.X, city.Y));
                city.SetGrassSiblings(GetSiblings(city.X, city.Y, BiomeType.grass));
            });
        }

        private void PlaceMainCastle()
        {
            int minBound = config.GetWaterEdge() + 5;
            int middle = config.GetWidth() / 2;
            int size = 3;

            List<Vector2Int> positions = new()
        {
            new (minBound, minBound), // south-west
            new (middle, minBound), // south
            new (config.GetWidth() - minBound - 1, minBound), // south-east

            new (minBound, middle), // west
            new (config.GetWidth() - minBound - 1, middle), // east

            new (minBound, config.GetHeight() - minBound - 1), // north-west
            new (middle, config.GetHeight() - minBound - 1), // north
            new (config.GetWidth() - minBound - 1, config.GetHeight() - minBound - 1), // north-east
        };

            while (positions.Count > 0)
            {
                int index = Random.Range(0, positions.Count);
                Vector2Int center = positions.ElementAt(index);
                positions.RemoveAt(index);

                if (IsEmptySquare(center.x, center.y, size))
                {
                    int x = center.x;
                    int y = center.y;

                    var castle = Castle.CreateRandom(x, y, config.GetNumber(), units, 0);
                    castle.SetOwner(CastleOwner.king);

                    grid.GetValue(x, y).ObjectLayer = castle;

                    grid.GetValue(x - 1, y + 1).ObjectLayer = CastleWall.Create(x - 1, y + 1, config.GetNumber(), 0);
                    grid.GetValue(x, y + 1).ObjectLayer = CastleWall.Create(x, y + 1, config.GetNumber(), 1);
                    grid.GetValue(x + 1, y + 1).ObjectLayer = CastleWall.Create(x + 1, y + 1, config.GetNumber(), 2);

                    grid.GetValue(x - 1, y).ObjectLayer = CastleWall.Create(x - 1, y, config.GetNumber(), 3);
                    grid.GetValue(x + 1, y).ObjectLayer = CastleWall.Create(x + 1, y, config.GetNumber(), 5);

                    castles.Add(castle);

                    // left tower
                    PlaceTowerAt(x - 3, y + 1);

                    // right tower
                    PlaceTowerAt(x + 2, y + 1);

                    // left bottom tower
                    PlaceTowerAt(x - 2, y - 1);

                    // right bottom tower
                    PlaceTowerAt(x + 1, y - 1);

                    // left top tower
                    PlaceTowerAt(x - 3, y + 3);

                    // right top tower
                    PlaceTowerAt(x + 2, y + 3);

                    positions.Clear();
                }
            }

        }

        private void PlaceTowerAt(int x, int y)
        {
            grid.GetValue(x, y).ObjectLayer = CastleWall.Create(x, y, config.GetNumber(), 0);
            grid.GetValue(x + 1, y).ObjectLayer = CastleWall.Create(x + 1, y, config.GetNumber(), 2);
            grid.GetValue(x, y - 1).ObjectLayer = CastleWall.Create(x, y - 1, config.GetNumber(), 3);
            grid.GetValue(x + 1, y - 1).ObjectLayer = CastleWall.Create(x + 1, y - 1, config.GetNumber(), 5);
        }

        private void PlaceMainCity()
        {
            var castle = castles.Where(castle => castle.GetOwner() == CastleOwner.king).FirstOrDefault();

            if (castle != null)
            {
                List<Vector2Int> positions = GetPointsWithWaterSiblings(castle.X, castle.Y, 5, 3);

                positions.Sort(delegate (Vector2Int one, Vector2Int two)
                {
                    int oneDist = (castle.X - one.x) * (castle.X - one.x) + (castle.Y - one.y) * (castle.Y - one.y);
                    int twoDist = (castle.X - two.x) * (castle.X - two.x) + (castle.Y - two.y) * (castle.Y - two.y);

                    if (oneDist < twoDist)
                    {
                        return -1;
                    }

                    if (oneDist > twoDist)
                    {
                        return 1;
                    }

                    return 0;
                });

                foreach (var position in positions)
                {
                    var tile = grid.GetValue(position.x, position.y);

                    if (tile.IsEmptyGround() &&
                        IsObjectNearby(ObjectType.castleGate, position, 2) == false &&
                        IsObjectNearby(ObjectType.city, position, 2) == false &&
                        IsObjectNearby(ObjectType.castleWall, position, 1) == false &&
                        IsBiomeNearby(BiomeType.water, position, 1, 3)
                    )
                    {
                        var randomSpell = SpellManager.Instance().GetRandomSpell();

                        var city = City.Create(position.x, position.y, config.GetNumber(), randomSpell);
                        tile.ObjectLayer = city;
                        cities.Add(city);

                        return;
                    }
                }
            }
        }

        private int ClearInconsistentTiles()
        {
            int checkIndex = 12;
            int numPatchedTiles = 0;

            for (int x = config.GetWaterEdge(); x < config.GetWidth() - config.GetWaterEdge(); x++)
            {
                for (int y = config.GetWaterEdge(); y < config.GetHeight() - config.GetWaterEdge(); y++)
                {
                    int index = grid.GetValue(x, y).BiomeLayer.GetTileIndex();
                    BiomeType biome = grid.GetValue(x, y).BiomeLayer.GetBiomeType();

                    if (
                        index == checkIndex &&
                        biome != BiomeType.grass &&
                        !CheckTileBelongsTo2x2Patch(x, y))
                    {
                        grid.GetValue(x, y).BiomeLayer.SetBiomeType(BiomeType.grass);

                        int grassIndex =
                            grid.GetValue(x, y + 1).HasObject(ObjectType.castleGate) ? 1 : 0;
                        grid.GetValue(x, y).BiomeLayer.SetTileIndex(grassIndex);

                        numPatchedTiles++;
                    }
                }
            }

            return numPatchedTiles;
        }

        private bool CheckTileBelongsTo2x2Patch(int x, int y)
        {
            return Check2x2Patch(x, y, PatchMode.LeftBottom)
                || Check2x2Patch(x, y, PatchMode.RightBottom)
                || Check2x2Patch(x, y, PatchMode.LeftTop)
                || Check2x2Patch(x, y, PatchMode.RightTop);
        }

        private bool Check2x2Patch(int x, int y, PatchMode mode)
        {
            int minX = -1, minY = -1, maxX = -1, maxY = -1;

            if (mode == PatchMode.LeftBottom)
            {
                minX = Mathf.Max(x, 0);
                minY = Mathf.Max(y, 0);
                maxX = Mathf.Min(x + 1, config.GetWidth() - 1);
                maxY = Mathf.Min(y + 1, config.GetHeight() - 1);
            }

            if (mode == PatchMode.RightBottom)
            {
                minX = Mathf.Max(x - 1, 0);
                minY = Mathf.Max(y, 0);
                maxX = Mathf.Min(x, config.GetWidth() - 1);
                maxY = Mathf.Min(y + 1, config.GetHeight() - 1);
            }

            if (mode == PatchMode.LeftTop)
            {
                minX = Mathf.Max(x, 0);
                minY = Mathf.Max(y - 1, 0);
                maxX = Mathf.Min(x + 1, config.GetWidth() - 1);
                maxY = Mathf.Min(y, config.GetHeight() - 1);
            }

            if (mode == PatchMode.RightTop)
            {
                minX = Mathf.Max(x - 1, 0);
                minY = Mathf.Max(y - 1, 0);
                maxX = Mathf.Min(x, config.GetWidth() - 1);
                maxY = Mathf.Min(y, config.GetHeight() - 1);
            }

            var biome = grid.GetValue(x, y).BiomeLayer.GetBiomeType();
            int biomeCounter = 0;

            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    if (grid.GetValue(i, j).BiomeLayer.GetBiomeType() == biome)
                    {
                        biomeCounter++;
                    }
                }
            }

            return biomeCounter == 4;
        }

        private bool IsBiomeNearby(BiomeType type, Vector2Int position, int radius, int siblingsCount = 1)
        {
            int minX = Mathf.Max(position.x - radius, 0);
            int maxX = Mathf.Min(position.x + radius, config.GetWidth() - 1);
            int minY = Mathf.Max(position.y - radius, 0);
            int maxY = Mathf.Min(position.y + radius, config.GetHeight() - 1);

            int totalCount = 0;
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (x != position.x &&
                        y != position.y &&
                        grid.GetValue(x, y).BiomeLayer.GetBiomeType() == type)
                    {
                        totalCount++;
                    }

                    if (totalCount >= siblingsCount)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsObjectNearby(ObjectType type, Vector2Int position, int radius)
        {
            int minX = Mathf.Max(position.x - radius, 0);
            int maxX = Mathf.Min(position.x + radius, config.GetWidth() - 1);
            int minY = Mathf.Max(position.y - radius, 0);
            int maxY = Mathf.Min(position.y + radius, config.GetHeight() - 1);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (grid.GetValue(x, y).HasObject(type))
                        return true;
                }
            }

            return false;
        }

        private List<Vector2Int> FreeNeighbours(
            Vector2Int xy,
            float continuity = 0.7f,
            BiomeType baseType = BiomeType.grass
        )
        {
            List<Vector2Int> neighbours = new();

            int startX = Mathf.Max(config.GetWaterEdge(), xy.x - 1);
            int startY = Mathf.Max(config.GetWaterEdge(), xy.y - 1);
            int endX = Mathf.Min(xy.x + 1, config.GetWidth() - config.GetWaterEdge());
            int endY = Mathf.Min(xy.y + 1, config.GetHeight() - config.GetWaterEdge());

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    if ((x != xy.x || y != xy.y) &&
                        grid.GetValue(x, y).IsEmptyGround(baseType) &&
                        Random.value < continuity)
                    {
                        neighbours.Add(new Vector2Int(x, y));
                    }
                }
            }

            return neighbours;
        }

        private List<Vector2Int> GetPointsWithWaterSiblings(int centerX, int centerY, int radius, int siblingsCount = 1)
        {
            int minX = Mathf.Max(centerX - radius, 0);
            int maxX = Mathf.Min(centerX + radius, config.GetWidth() - 1);
            int minY = Mathf.Max(centerY - radius, 0);
            int maxY = Mathf.Min(centerY + radius, config.GetHeight() - 1);

            List<Vector2Int> siblings = new();
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (grid.GetValue(x, y).BiomeLayer.GetBiomeType() == BiomeType.grass &&
                        !grid.GetValue(x, y).HasObject() &&
                        IsBiomeNearby(BiomeType.water, new Vector2Int(x, y), siblingsCount))
                    {
                        siblings.Add(new Vector2Int(x, y));
                    }
                }
            }

            return siblings;
        }

        private List<Vector2Int> GetSiblings(int centerX, int centerY, BiomeType type = BiomeType.water)
        {
            int minX = Mathf.Max(centerX - 1, 0);
            int maxX = Mathf.Min(centerX + 1, config.GetWidth() - 1);
            int minY = Mathf.Max(centerY - 1, 0);
            int maxY = Mathf.Min(centerY + 1, config.GetHeight() - 1);

            List<Vector2Int> siblings = new();

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (x != centerX &&
                        y != centerY &&
                        grid.GetValue(x, y).BiomeLayer.GetBiomeType().Equals(type))
                    {
                        siblings.Add(new Vector2Int(x, y));
                    }
                }
            }

            return siblings;
        }
        private Vector2Int RandomPointFor2x2Patch(BiomeType baseType = BiomeType.grass)
        {
            int numIterations = 0;

            Vector2Int point = new();

            while (numIterations < MAX_ITERATIONS_COUNT)
            {
                point = RandomPoint(baseType);

                if (
                    point.x < config.GetWidth() - config.GetWaterEdge() &&
                    point.y < config.GetHeight() - config.GetWaterEdge()
                    )
                {
                    numIterations += MAX_ITERATIONS_COUNT;
                }
                else
                {
                    numIterations++;
                }
            }

            return point;
        }

        private Vector2Int RandomPoint(BiomeType baseType = BiomeType.grass)
        {
            int numIterations = 0;
            Vector2Int point = new();

            while (numIterations < MAX_ITERATIONS_COUNT)
            {
                RandomPointWithinShores(out int x, out int y);

                ContinentTile currentTile = grid.GetValue(x, y);
                ContinentTile topTile = grid.GetValue(x, y + 1);

                if (currentTile.IsEmptyGround(baseType) &&
                    topTile.HasObject(ObjectType.castleGate) == false)
                {
                    point.x = x;
                    point.y = y;
                    numIterations += MAX_ITERATIONS_COUNT;
                }
                else
                {
                    numIterations++;
                }
            }

            return point;
        }

        private bool IsWithinShores(int x, int y)
        {
            return
                x >= config.GetWaterEdge()
                && y >= config.GetWaterEdge()
                && x <= config.GetWidth() - config.GetWaterEdge() - 1
                && y <= config.GetHeight() - config.GetWaterEdge() - 1
                ;
        }

        private void RandomPointWithinShores(out int x, out int y, int additionalBound = 0)
        {
            int bounds = config.GetWaterEdge() + additionalBound;

            x = Random.Range(bounds, config.GetWidth() - bounds);
            y = Random.Range(bounds, config.GetHeight() - bounds);
        }

        private bool IsEmptySquare(int centerX, int centerY, int radius, BiomeType baseType = BiomeType.grass)
        {
            int minX = Mathf.Max(centerX - radius, 0);
            int maxX = Mathf.Min(centerX + radius, config.GetWidth() - 1);
            int minY = Mathf.Max(centerY - radius, 0);
            int maxY = Mathf.Min(centerY + radius, config.GetHeight() - 1);

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    if (grid.GetValue(x, y).IsEmptyGround(baseType) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private List<Vector2Int> GetShorePoints()
        {
            List<Vector2Int> points = new();
            for (int x = config.GetWaterEdge(); x < config.GetWidth() - config.GetWaterEdge(); x++)
            {
                for (int y = config.GetWaterEdge(); y < config.GetHeight() - config.GetWaterEdge(); y++)
                {
                    if (grid.GetValue(x, y).IsEmptyGround() &&
                        IsBiomeNearby(BiomeType.water, new Vector2Int(x, y), 3))
                    {
                        points.Add(new Vector2Int(x, y));
                    }
                }
            }

            return points;
        }
    }
}