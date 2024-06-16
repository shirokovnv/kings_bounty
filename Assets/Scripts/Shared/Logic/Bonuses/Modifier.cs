namespace Assets.Scripts.Shared.Logic.Bonuses
{
    [System.Serializable]
    abstract public class Modifier<T> : Bonus
    {
        protected Modifier(bool isStackable) : base(isStackable)
        {
        }

        abstract public T GetValue();
    }
}