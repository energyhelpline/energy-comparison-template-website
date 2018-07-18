using BareboneUi.Common;
using System.Collections.Generic;

namespace BareboneUi.Pages.CurrentUsage
{
    public class CurrentUsageViewModel : ViewModelWithErrors, ICurrentUsageAnswer
    {
        public string CurrentUsageUri { get; set; }
        public string GasUsageType { get; set; }
        public string ElectricityUsageType { get; set; }
        public string GasUsageSimpleEstimator { get; set; }
        public string NightPercentageUsage { get; set; }
        public string ElectricityUsageSimpleEstimator { get; set; }
        public string HouseType { get; set; }
        public string NumberOfBedrooms { get; set; }
        public string MainCookingSource { get; set; }
        public string CookingFrequency { get; set; }
        public string CentralHeating { get; set; }
        public string NumberOfOccupants { get; set; }
        public string Insulation { get; set; }
        public string EnergyUsage { get; set; }
        public string GasCurrentSpend { get; set; }
        public string ElectricityCurrentSpend { get; set; }
        public string GasCurrentUsageAsKwh { get; set; }
        public string ElectricityCurrentUsageAsKwh { get; set; }
        public IEnumerable<KeyPair> GasUsageSimpleEstimators { get; set; }
        public IEnumerable<KeyPair> ElectricityUsageSimpleEstimators { get; set; }
        public IEnumerable<KeyPair> HouseTypeValues { get; set; }
        public IEnumerable<KeyPair> NumberOfBedroomsValues { get; set; }
        public IEnumerable<KeyPair> MainCookingSourceValues { get; set; }
        public IEnumerable<KeyPair> CookingFrequencyValues { get; set; }
        public IEnumerable<KeyPair> CentralHeatingValues { get; set; }
        public IEnumerable<KeyPair> NumberOfOccupantsValues { get; set; }
        public IEnumerable<KeyPair> InsulationValues { get; set; }
        public IEnumerable<KeyPair> EnergyUsageValues { get; set; }
    }
}
