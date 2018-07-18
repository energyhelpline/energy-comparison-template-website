namespace BareboneUi.Pages.CurrentSupply
{
    public interface ICurrentSupplyAnswer
    {
        bool IsGasComparison { get; }
        bool IsElectricityComparison { get; }
        bool Economy7 { get; }
        string GasSupplier { get; }
        string GasSupplierTariff { get; }
        string GasSupplierPaymentMethod { get; }
        string ElectricitySupplier { get; }
        string ElectricitySupplierTariff { get; }
        string ElectricitySupplierPaymentMethod { get; }
    }
}