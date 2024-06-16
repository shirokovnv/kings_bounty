namespace Assets.Scripts.Adventure.Logic.Accounting
{
    public interface ITaxable
    {
        public int GetTax();
        public string GetTaxInfo();
    }
}