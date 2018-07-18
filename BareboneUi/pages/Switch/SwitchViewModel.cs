namespace BareboneUi.Pages.Switch
{
    public class SwitchViewModel
    {
        public string Postcode { get; set; }

        public string GasSupplyName { get; set; }
        public string GasTariff { get; set; }
        public string GasPaymentMethod { get; set; }
        public string GasContractExpiryDate { get; set; }
        public string ElectricitySupplyName { get; set; }
        public string ElectricityTariff { get; set; }
        public string ElectricityPaymentMethod { get; set; }
        public string ElectricityContractExpiryDate { get; set; }
        public string NightPercentUsage { get; set; }
        public string ElectricityEcomony7 { get; set; }

        public string GasUsageSimpleEstimator { get; set; }
        public string ElectricityUsageSimpleEstimator { set; get; }

        public string GasUsageType { get; set; }
        public string GasHouseType { get; set; }
        public string GasNumberOfBedrooms { get; set; }
        public string GasMainCookingSource { get; set; }
        public string GasCookingFrequency { get; set; }
        public string GasCentralHeating { get; set; }
        public string GasNumberOfOccupants { get; set; }
        public string GasInsulation { get; set; }
        public string GasEnergyUsage { get; set; }
        public string ElectricityUsageType { get; set; }
        public string ElectricityHouseType { get; set; }
        public string ElectricityNumberOfBedrooms { get; set; }
        public string ElectricityMainCookingSource { get; set; }
        public string ElectricityCookingFrequency { get; set; }
        public string ElectricityCentralHeating { get; set; }
        public string ElectricityNumberOfOccupants { get; set; }
        public string ElectricityInsulation { get; set; }
        public string ElectricityEnergyUsage { get; set; }

        public string GasCurrentSpend { get; set; }
        public string ElectricityCurrentSpend { get; set; }

        public string GasCurrentUsageAsKwh { get; set; }
        public string ElectricityCurrentUsageAsKwh { get; set; }

        public string TransferUrl { get; set; }
    }
}
