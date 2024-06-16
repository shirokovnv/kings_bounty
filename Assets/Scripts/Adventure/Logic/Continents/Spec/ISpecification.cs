namespace Assets.Scripts.Adventure.Logic.Continents.Spec
{
    public interface ISpecification<in T> where T : class
    {
        public bool IsSatisfiedBy(T item);
    }
}