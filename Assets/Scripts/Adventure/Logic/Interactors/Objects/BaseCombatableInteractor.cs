namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Objects
{
    abstract public class BaseCombatableInteractor : BaseObjectInteractor
    {
        protected string ApproximateQuantity(int quantity)
        {
            if (quantity >= 1 && quantity <= 9)
            {
                return "A few";
            }

            if (quantity >= 10 && quantity <= 19)
            {
                return "Some";
            }

            if (quantity >= 20 && quantity <= 49)
            {
                return "Many";
            }

            if (quantity >= 50 && quantity <= 99)
            {
                return "A lot of";
            }

            if (quantity >= 100 && quantity <= 499)
            {
                return "A horde of";
            }

            return "A multitude of";
        }
    }
}