using Assets.Scripts.Shared.Grid;

namespace Assets.Scripts.Adventure.Logic.Puzzle
{
    [System.Serializable]
    public class PuzzleInfo
    {
        public int Width, Height;
        public int CenterX, CenterY, ContinentNumber;
        public TileGrid<PuzzlePiece> Puzzle;

        public PuzzleInfo(int width, int height, int centerX, int centerY, int continentNumber)
        {
            Width = width;
            Height = height;
            CenterX = centerX;
            CenterY = centerY;
            ContinentNumber = continentNumber;
            Puzzle = new TileGrid<PuzzlePiece>(width, height, (g, x, y) => null);
        }

        public PuzzlePiece GetPuzzlePieceByID(int id, PuzzlePiece.PuzzleType type)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var puzzlePiece = Puzzle.GetValue(x, y);
                    if (puzzlePiece.Type == type && puzzlePiece.ID == id)
                    {
                        return puzzlePiece;
                    }
                }
            }

            return null;
        }
    }
}