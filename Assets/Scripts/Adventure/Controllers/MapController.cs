using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Continents;
using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.Logic.Continents.Object.Biome;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Combat;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Grid;
using Assets.Scripts.Shared.Logic.Character;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Controllers
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] protected int width, height;
        [SerializeField] protected GridTile tilePrefab;

        [SerializeField] protected Sprite[] desert;
        [SerializeField] protected Sprite[] grass;
        [SerializeField] protected Sprite[] mountain;
        [SerializeField] protected Sprite[] water;
        [SerializeField] protected Sprite[] forest;

        [SerializeField] protected Sprite[] castle;
        [SerializeField] protected Sprite[] dwellings;
        [SerializeField] protected Sprite[] artifacts;

        [SerializeField] protected Sprite[] captains;
        [SerializeField] protected Sprite chest;
        [SerializeField] protected Sprite sign;
        [SerializeField] protected Sprite city;

        [SerializeField] protected float scaleX;
        [SerializeField] protected float scaleY;

        protected List<TileGrid<ContinentTile>> gridList;
        protected int gridListIndex;

        public static MapController Instance;

        private void Awake()
        {
            EventBus.Instance.Register(this);
            Instance = this;
        }

        private void DestroyContinentTiles(int index)
        {
            var gridTile = gridList[index];

            for (int x = 0; x < gridTile.GetWidth(); x++)
            {
                for (int y = 0; y < gridTile.GetHeight(); y++)
                {
                    Destroy(GameObject.Find(TileNameKey(x, y, index)));
                }
            }
        }

        private void CreateContinentTiles(int index)
        {
            var continentTiles = gridList[index];

            for (int x = 0; x < continentTiles.GetWidth(); x++)
            {
                for (int y = 0; y < continentTiles.GetHeight(); y++)
                {
                    var tile = InitWorldTile(x, y, continentTiles.GetValue(x, y));
                    tile.name = TileNameKey(x, y, index);
                }
            }
        }

        private void ReplaceObjectTile(int x, int y, int continentNumber)
        {
            var obj = GameObject.Find(TileNameKey(x, y, continentNumber - 1));

            if (obj != null)
            {
                var tile = obj.GetComponent<GridTile>();
                tile.SetSprite(GetSpriteByGridTile(x, y, gridList[continentNumber - 1].GetValue(x, y)));
            }
        }

        private GridTile InitWorldTile(int x, int y, ContinentTile tile)
        {
            var position = new Vector3Int(x, y);

            GridTile tileView = Instantiate(tilePrefab, position, Quaternion.identity);
            tileView.SetSprite(GetSpriteByGridTile(x, y, tile));
            tileView.SetTextMeshActive(false);
            tileView.SetSpriteScaleRatio(1, 1);

            return tileView;
        }

        public Sprite GetSpriteByGridTile(int x, int y, ContinentTile tile, bool checkObject = true)
        {
            if (tile == null)
            {
                return null;
            }

            if (tile.HasObject() && checkObject)
            {
                var objectType = tile.ObjectLayer.GetObjectType();

                switch (objectType)
                {
                    case ObjectType.dwelling:
                        Dwelling dwelling = (Dwelling)tile.ObjectLayer;
                        Dwelling.DwellingType dwellingType = dwelling.GetDwellingType();
                        return GetSpriteByDwellingType(dwellingType);

                    case ObjectType.city:
                        return city;

                    case ObjectType.chest:
                        return chest;

                    case ObjectType.captain:
                        Captain captain = (Captain)tile.ObjectLayer;

                        return captains[captain.ContinentNumber - 1];

                    case ObjectType.sign:
                        return sign;

                    case ObjectType.artifact:
                        Artifact artifact = (Artifact)tile.ObjectLayer;
                        return GetSpriteByArtifact(artifact);

                    case ObjectType.castleWall:
                        CastleWall castleWall = (CastleWall)tile.ObjectLayer;
                        return castle[castleWall.GetTileIndex()];
                    case ObjectType.castleGate:
                        Castle castleGate = (Castle)tile.ObjectLayer;
                        return castle[castleGate.GetTileIndex()];
                }
            }

            var type = tile.BiomeLayer.GetBiomeType();

            int index;
            switch (type)
            {
                case BiomeType.water:
                    index = tile.BiomeLayer.GetTileIndex();
                    return water[index];
                case BiomeType.forest:
                    index = tile.BiomeLayer.GetTileIndex();
                    return forest[index];
                case BiomeType.grass:
                    index = tile.BiomeLayer.GetTileIndex();
                    return grass[index];
                case BiomeType.mountain:
                    index = tile.BiomeLayer.GetTileIndex();
                    return mountain[index];
                case BiomeType.desert:
                    index = tile.BiomeLayer.GetTileIndex();
                    return desert[index];
            }

            return null;
        }

        private Sprite GetSpriteByDwellingType(Dwelling.DwellingType type)
        {
            return type switch
            {
                Dwelling.DwellingType.plains => dwellings[0],
                Dwelling.DwellingType.forest => dwellings[1],
                Dwelling.DwellingType.cavern => dwellings[2],
                Dwelling.DwellingType.dungeon => dwellings[3],
                _ => null,
            };
        }

        private Sprite GetSpriteByArtifact(Artifact artifact)
        {
            return artifact.ArtifactScript.Name switch
            {
                "Sword of Prowess" => artifacts[0],
                "Shield of Protection" => artifacts[1],
                "Anchor of Admiralty" => artifacts[2],
                "Crown of Command" => artifacts[3],
                "Tome of Knowledge" => artifacts[4],
                "Amulet of Augmentation" => artifacts[5],
                "Ring of Snake" => artifacts[6],
                "Articles of Nobility" => artifacts[7],
                _ => null,
            };
        }

        private string TileNameKey(int x, int y, int index)
        {
            return x + "," + y + "," + (index + 1);
        }

        public void OnEvent(OnChangeContinent e)
        {
            DestroyContinentTiles(gridListIndex);
            gridListIndex = e.ContinentNumber - 1;
            CreateContinentTiles(gridListIndex);
        }

        public void OnEvent(OnPickObject e)
        {
            ReplaceObjectTile(e.X, e.Y, e.ContinentNumber);
        }

        public void OnEvent(OnMoveCaptain e)
        {
            ReplaceObjectTile(e.X, e.Y, e.ContinentNumber);
            ReplaceObjectTile(e.TargetX, e.TargetY, e.ContinentNumber);
        }

        public void OnEvent(OnJoinSquad e)
        {
            ReplaceObjectTile(e.X, e.Y, e.ContinentNumber);
        }

        public void OnEvent(OnWinCombat e)
        {
            var obj = e.Combatable.GetObject();

            OnAfterSceneChanged();
            ReplaceObjectTile(obj.X, obj.Y, obj.ContinentNumber);
        }

        public void OnEvent(OnLoseCombat e)
        {
            gridList = ContinentSystem.Instance().GetContinents().Select(x => x.GetGrid()).ToList();
            gridListIndex = Player.Instance().ContinentNumber - 1;
        }

        public void OnEvent(OnNewGame e)
        {
            OnAfterSceneChanged();
        }

        public void OnEvent(OnLoadGame e)
        {
            OnAfterSceneChanged();
        }

        private void OnAfterSceneChanged()
        {
            gridList = ContinentSystem.Instance().GetContinents().Select(x => x.GetGrid()).ToList();
            gridListIndex = Player.Instance().ContinentNumber - 1;

            if (gridList.Count > 0)
            {
                CreateContinentTiles(gridListIndex);
            }
        }
    }
}