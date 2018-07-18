namespace BareboneUi.Pages.FutureSupply
{
    public class EnergySupply
    {
        public string Id { get; set; }
        public bool CanApply { get; set; }
        public decimal ExpectedAnnualSavings { get; set; }
        public SupplyDetails SupplyDetails { get; set; }
        public Supplier Supplier { get; set; }
    }
}