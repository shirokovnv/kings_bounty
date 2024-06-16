using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Shared.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.UI.RightPanel
{
    public class MiniPuzzleUI : MonoBehaviour
    {
        [SerializeField] private BrickScript brick;
        private List<BrickScript> bricks;

        private void Awake()
        {
            bricks = new List<BrickScript>();

            EventBus.Instance.Register(this);
        }

        public void OnEvent(OnPuzzleCreated e)
        {
            bricks.ForEach(brick => Destroy(brick.gameObject));
            bricks.Clear();

            for (int y = 0; y < e.PuzzleInfo.Height; y++)
            {
                for (int x = 0; x < e.PuzzleInfo.Width; x++)
                {
                    var newBrick = Instantiate(brick);
                    var piece = e.PuzzleInfo.Puzzle.GetValue(x, e.PuzzleInfo.Height - 1 - y);

                    newBrick.transform.SetParent(transform);
                    newBrick.name = $"brick#{piece.ID}";
                    newBrick.PuzzlePiece = piece;
                    newBrick.Toggle(!piece.Revealed);
                    newBrick.transform.localScale = new Vector3(1, 1, 1);

                    bricks.Add(newBrick);
                }
            }
        }

        public void OnEvent(OnPuzzleRevealed e)
        {
            var revealedBrick = bricks.Where(p => p.PuzzlePiece.ID == e.PuzzlePiece.ID).FirstOrDefault();

            if (revealedBrick != null)
            {
                revealedBrick.Toggle(false);
            }
        }
    }
}