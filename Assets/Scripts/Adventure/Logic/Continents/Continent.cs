using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Continents.Config;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.Logic.Continents.Object.Biome;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents
{
    [System.Serializable]
    public class Continent : NamedObject, ISerializationCallbackReceiver
    {
        [SerializeField] private TileGrid<ContinentTile> grid;
        [SerializeField] private ContinentConfig config;
        [SerializeField] private bool isRevealed;
        [SerializeField] private bool hasFullMap;

        private List<Castle> castles;
        private List<City> cities;

        public Continent(string name, ContinentConfig config, TileGrid<ContinentTile> grid) : base(name)
        {
            this.config = config;
            this.grid = grid;

            isRevealed = false;
            hasFullMap = false;

            InspectContinent();
            LinkCitiesAndCastles();
        }

        public ContinentConfig GetConfig() { return config; }
        public TileGrid<ContinentTile> GetGrid() { return grid; }

        public bool IsRevealed() { return isRevealed; }
        public void SetRevealed(bool revealed) { isRevealed = revealed; }

        public bool HasFullMap()
        {
            return hasFullMap;
        }

        public void SetHasFullMap(bool hasFullMap)
        {
            this.hasFullMap = hasFullMap;
        }

        public List<Castle> GetCastles()
        {
            return castles;
        }

        public List<City> GetCities()
        {
            return cities;
        }

        public List<BaseObject> GetObjectsOfType(ObjectType type)
        {
            var objects = new List<BaseObject>();

            for (int x = config.GetWaterEdge(); x < grid.GetWidth() - config.GetWaterEdge(); x++)
            {
                for (int y = config.GetWaterEdge(); y < grid.GetHeight() - config.GetWaterEdge(); y++)
                {
                    var tile = grid.GetValue(x, y);

                    if (tile.HasObject(type))
                    {
                        objects.Add(tile.ObjectLayer);
                    }
                }
            }

            return objects;
        }

        // TODO: refactor
        public Vector2Int GetStartPosition()
        {
            var kingsCastle = castles.Where(castle => castle.GetOwner() == CastleOwner.king).FirstOrDefault();

            if (kingsCastle != null)
            {
                return new Vector2Int(kingsCastle.X, kingsCastle.Y - 1);
            }

            return new Vector2Int(0, 0);
        }

        private void InspectContinent()
        {
            castles = new List<Castle>();
            cities = new List<City>();

            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    var tile = grid.GetValue(x, y);

                    if (tile.HasObject(ObjectType.castleGate))
                    {
                        castles.Add((Castle)tile.ObjectLayer);
                    }

                    if (tile.HasObject(ObjectType.city))
                    {
                        cities.Add((City)tile.ObjectLayer);
                    }
                }
            }
        }

        public List<Vector2Int> GetAllGroundPositionsExceptCastleGate()
        {
            var positions = new List<Vector2Int>();
            for (int x = config.GetWaterEdge(); x < grid.GetWidth() - config.GetWaterEdge(); x++)
            {
                for (int y = config.GetWaterEdge(); y < grid.GetHeight() - config.GetWaterEdge(); y++)
                {
                    if (
                        grid.GetValue(x, y).ObjectLayer == null &&
                        grid.GetValue(x, y).BiomeLayer.GetBiomeType().Equals(BiomeType.grass) &&
                        !grid.GetValue(x, y + 1).HasObject(ObjectType.castleGate)
                        )
                    {
                        positions.Add(new Vector2Int(x, y));
                    }
                }
            }

            return positions;
        }

        public List<ContinentTilePackage> GetTileRectWithPositions(int centerX, int centerY, int rX, int rY)
        {
            var tiles = new List<ContinentTilePackage>();

            int minX = Mathf.Max(0, centerX - rX);
            int minY = Mathf.Max(0, centerY - rY);
            int maxX = Mathf.Min(grid.GetWidth() - 1, centerX + rX);
            int maxY = Mathf.Min(grid.GetHeight() - 1, centerY + rY);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    tiles.Add(new ContinentTilePackage
                    {
                        X = x,
                        Y = y,
                        Tile = grid.GetValue(x, y)
                    });
                }
            }

            return tiles;
        }

        private void LinkCitiesAndCastles()
        {
            if (castles.Count != cities.Count)
            {
                return;
            }

            var tempCastles = new List<Castle>(castles);

            for (int i = 0; i < cities.Count; i++)
            {
                City city = cities.ElementAt(i);

                tempCastles.Sort(delegate (Castle one, Castle two)
                {
                    int oneDist = (city.X - one.X) * (city.X - one.X) + (city.Y - one.Y) * (city.Y - one.Y);
                    int twoDist = (city.X - two.X) * (city.X - two.X) + (city.Y - two.Y) * (city.Y - two.Y);

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

                var castle = tempCastles.ElementAt(0);

                city.SetLinkedCastle(castle);
                tempCastles.RemoveAt(0);
            }
        }

        public void ChangeTilesVisibility(Vector2Int playerPosition, int radius, bool isRevealed)
        {
            int pX = playerPosition.x;
            int pY = playerPosition.y;

            int minX = Mathf.Max(0, pX - radius);
            int minY = Mathf.Max(0, pY - radius);
            int maxX = Mathf.Min(grid.GetWidth() - 1, pX + radius);
            int maxY = Mathf.Min(grid.GetHeight() - 1, pY + radius);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    grid.GetValue(x, y).SetRevealed(isRevealed);
                }
            }
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            InspectContinent();
            LinkCitiesAndCastles();
        }
    }
}