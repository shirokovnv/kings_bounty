namespace Assets.Scripts.Combat.Logic.AI.Grid
{
    public class CombatGridCell
    {
        int x, y;
        bool obstacle;

        public CombatGridCell(int x, int y, bool obstacle = false)
        {
            this.x = x;
            this.y = y;
            this.obstacle = obstacle;
        }

        public void SetObstacle(bool obstacle = false)
        {
            this.obstacle = obstacle;
        }

        public bool IsObstacle() { return obstacle; }

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
    }
}