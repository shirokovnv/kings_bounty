using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Puzzle;
using Assets.Scripts.Shared.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Systems
{
    [System.Serializable]
    public class PuzzleSystem : ISerializationCallbackReceiver
    {
        private static PuzzleSystem instance;
        [SerializeField] private PuzzleInfo puzzleInfo;

        public static PuzzleSystem Instance()
        {
            instance ??= new PuzzleSystem();

            return instance;
        }

        public void Create(IPuzzleBag puzzleBag, IPuzzleLocator puzzleLocator)
        {
            var puzzles = puzzleBag.GetPuzzles();
            int size = (int)Mathf.Sqrt(puzzles.Count());
            var (center, continentNumber) = puzzleLocator.SelectCenter();

            if (size * size != puzzles.Count())
            {
                throw new System.Exception("Puzzles count size is incorrect.");
            }

            puzzleInfo = new PuzzleInfo(size, size, center.x, center.y, continentNumber);

            List<Vector2Int> positions = new();
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    positions.Add(new Vector2Int(x, y));
                }
            }

            positions = positions.OrderBy(_ => Random.value).ToList();

            Dictionary<Vector2Int, PuzzlePiece> puzzleMap = positions
                .Zip(puzzles, (key, value) => new { Key = key, Value = value })
                .ToDictionary(item => item.Key, item => item.Value);

            foreach (var kvp in puzzleMap)
            {
                kvp.Value.Revealed = false;

                puzzleInfo.Puzzle.SetValue(kvp.Key.x, kvp.Key.y, kvp.Value);
            }

            EventBus.Instance.PostEvent(new OnPuzzleCreated { PuzzleInfo = puzzleInfo });
        }

        public void RevealPuzzlePiece(int id, PuzzlePiece.PuzzleType type)
        {
            var piece = puzzleInfo.GetPuzzlePieceByID(id, type);

            if (piece != null)
            {
                piece.Revealed = true;

                EventBus.Instance.PostEvent(new OnPuzzleRevealed { PuzzlePiece = piece });
            }
        }

        public PuzzleInfo GetPuzzleInfo()
        {
            return puzzleInfo;
        }

        public void OnAfterDeserialize()
        {
            instance = this;
        }

        public void OnBeforeSerialize()
        {
        }
    }
}