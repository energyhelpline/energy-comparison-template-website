namespace BareboneUi.Pages.CurrentUsage
{
    public interface ICurrentUsageAnswer
    {
        string GasUsageType { get; }
        string ElectricityUsageType { get; }
        string GasUsageSimpleEstimator { get; }
        string NightPercentageUsage { get; }
        string ElectricityUsageSimpleEstimator { get; }
        string HouseType { get; }
        string NumberOfBedrooms { get; }
        string MainCookingSource { get; }
        string CookingFrequency { get; }
        string CentralHeating { get; }
        string NumberOfOccupants { get; }
        string Insulation { get; }
        string EnergyUsage { get; }
        string GasCurrentSpend { get; }
        string ElectricityCurrentSpend { get; }
        string GasCurrentUsageAsKwh { get; }
        string ElectricityCurrentUsageAsKwh { get; }
    }
}