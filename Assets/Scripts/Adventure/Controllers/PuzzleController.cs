using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Continents;
using Assets.Scripts.Adventure.Logic.Puzzle;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Combat;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Grid;
using Assets.Scripts.Shared.Logic.Character;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Controllers
{
    public class PuzzleController : MonoBehaviour
    {
        protected struct PuzzleInfoPackage
        {
            public GridTile PuzzleObject;
            public int X, Y;
            public TileGrid<ContinentTile> TileGrid;
        }

        [SerializeField] protected Sprite puzzleSprite;
        [SerializeField] protected GridTile tilePrefab;

        protected List<PuzzleInfoPackage> revealedPuzzles;
        protected float timer;
        protected float revealDelay = 0.3f;

        private void Awake()
        {
            revealedPuzzles = new List<PuzzleInfoPackage>();
            EventBus.Instance.Register(this);
            timer = 0;
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;

            if (timer > revealDelay)
            {
                timer -= revealDelay;

                if (revealedPuzzles.Count > 0)
                {
                    var revealedPuzzle = revealedPuzzles.ElementAt(0);
                    revealedPuzzle.PuzzleObject.EndAnimation();

                    var (x, y, grid) = (revealedPuzzle.X, revealedPuzzle.Y, revealedPuzzle.TileGrid);

                    revealedPuzzle.PuzzleObject.SetSprite(
                        MapController.Instance.GetSpriteByGridTile(x, y, grid.GetValue(x, y), false)
                        );

                    revealedPuzzles.RemoveAt(0);
                }
            }
        }

        public void OnEvent(OnViewPuzzleMap e)
        {
            PuzzleInfo puzzleInfo = PuzzleSystem.Instance().GetPuzzleInfo();
            Continent continent = ContinentSystem.Instance().GetContinentAtNumber(puzzleInfo.ContinentNumber);

            var tilePack = continent.GetTileRectWithPositions(
                puzzleInfo.CenterX, puzzleInfo.CenterY,
                puzzleInfo.Width, puzzleInfo.Height
                );

            int deltaX = Player.Instance().X - puzzleInfo.CenterX;
            int deltaY = Player.Instance().Y - puzzleInfo.CenterY;

            var grid = continent.GetGrid();

            for (int i = 0; i < tilePack.Count; i++)
            {
                int x = tilePack[i].X;
                int y = tilePack[i].Y;

                var tile = InitWorldTile(
                    x + deltaX,
                    y + deltaY,
                    grid.GetValue(x + deltaX, y + deltaY)
                );
                tile.name = $"puzzle#{i}";

                var puzzlePiece = puzzleInfo.Puzzle.GetValue(
                    x - puzzleInfo.CenterX + puzzleInfo.Width / 2,
                    y - puzzleInfo.CenterY + puzzleInfo.Height / 2
                    );

                if (puzzlePiece != null)
                {
                    var sprites = GetSpritesByPuzzlePiece(puzzlePiece);

                    tile.SetSprite(sprites[0]);
                    tile.BeginAnimation(sprites);

                    if (puzzlePiece.Revealed)
                    {
                        revealedPuzzles.Add(new PuzzleInfoPackage
                        {
                            X = x,
                            Y = y,
                            TileGrid = grid,
                            PuzzleObject = tile,
                        });
                    }
                }
                else
                {
                    tile.SetSprite(puzzleSprite);
                }
            }
        }

        public void OnEvent(OnDidViewPuzzleMap e)
        {
            PuzzleInfo puzzleData = PuzzleSystem.Instance().GetPuzzleInfo();
            Continent continent = ContinentSystem.Instance().GetContinentAtNumber(puzzleData.ContinentNumber);

            var tilePack = continent.GetTileRectWithPositions(
                puzzleData.CenterX, puzzleData.CenterY,
                puzzleData.Width, puzzleData.Height
                );

            for (int i = 0; i < tilePack.Count; i++)
            {
                Destroy(GameObject.Find($"puzzle#{i}"));
            }
        }

        private Sprite[] GetSpritesByPuzzlePiece(PuzzlePiece puzzlePiece)
        {
            return puzzlePiece.Type switch
            {
                PuzzlePiece.PuzzleType.Artifact => new[] { ArtifactManager.Instance().GetArtifactByID(puzzlePiece.ID).InventorySprite },
                PuzzlePiece.PuzzleType.Contract => ContractSystem.Instance().GetContractByID(puzzlePiece.ID).ContractInfo().Sprites,
                _ => null
            };
        }

        private GridTile InitWorldTile(int x, int y, ContinentTile tile)
        {
            var position = new Vector3Int(x, y);

            GridTile tileView = Instantiate(tilePrefab, position, Quaternion.identity);
            tileView.SetSprite(MapController.Instance.GetSpriteByGridTile(x, y, tile, false));
            tileView.SetTextMeshActive(false);
            tileView.SetSpriteScaleRatio(1, 1);
            tileView.SetSortingOrder(400);

            return tileView;
        }
    }
}