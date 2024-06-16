namespace Assets.Scripts.Adventure.Logic.Puzzle
{
    [System.Serializable]
    public class PuzzlePiece
    {
        public enum PuzzleType
        {
            Artifact,
            Contract,
        }

        public PuzzleType Type;
        public int ID;
        public bool Revealed;
    }
}