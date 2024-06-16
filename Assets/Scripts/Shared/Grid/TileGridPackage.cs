namespace Assets.Scripts.Shared.Grid
{
    [System.Serializable]
    struct TileGridPackage<TGridObject>
    {
        public int X, Y;
        public TGridObject GridObject;

        public TileGridPackage(int x, int y, TGridObject gridObject)
        {
            X = x;
            Y = y;
            GridObject = gridObject;
        }
    }
}