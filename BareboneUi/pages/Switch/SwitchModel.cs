using System;
using System.Linq;

namespace BareboneUi.Pages.Switch
{
    public class SwitchModel
    {
        private readonly SwitchResource _switchResource;
        private readonly Links _links;

        public SwitchModel(SwitchResource switchResource)
        {
            _switchResource = switchResource;
            _links = new Links(switchResource.Links);
        }

        public string Postcode => _switchResource.SupplyLocation?.SupplyPostcode;
        public string GasSupplyName => _switchResource.CurrentSupply?.Gas.Supplier.Name;
        public string GasTariff => _switchResource.CurrentSupply?.Gas.SupplierTariff.Name;
        public string GasPaymentMethod => _switchResource.CurrentSupply?.Gas.PaymentMethod.Name;
        public string ElectricitySupplyName => _switchResource.CurrentSupply?.Electricity.Supplier.Name;
        public string ElectricityTariff => _switchResource.CurrentSupply?.Electricity.SupplierTariff.Name;
        public string ElectricityPaymentMethod => _switchResource.CurrentSupply?.Electricity.PaymentMethod.Name;
        public bool IsProRata => _links.ContainRel("/rels/domestic/contract-details");
        public DateTime? GasContractExpiryDate => new JsonDate(_switchResource.CurrentSupply?.Gas.ContractExpiryDate).ToDateTime();
        public DateTime? ElectricityContractExpiryDate => new JsonDate(_switchResource.CurrentSupply?.Electricity.ContractExpiryDate).ToDateTime();
        public bool? ElectricityEcomony7 => _switchResource.CurrentSupply?.Electricity.Economy7;

        public string CurrentUsageUri => _links["/rels/domestic/usage"];
        public string ContractExpiryDateUri => _links["/rels/domestic/contract-details"];

        public string GetGasUsageSimpleEstimator() => SimpleEstimate(_switchResource.CurrentUsage?.Gas);
        public string ElectricityUsageSimpleEstimator => SimpleEstimate(_switchResource.CurrentUsage?.Elec);

        public string GetGasUsageType() => UsageType(_switchResource.CurrentUsage?.Gas);
        public string GetGasHouseType() => HouseType(_switchResource.CurrentUsage?.Gas);
        public string GetGasNumberOfBedrooms() => NumberOfBedrooms(_switchResource.CurrentUsage?.Gas);
        public string GetGasMainCookingSource() => MainCookingSource(_switchResource.CurrentUsage?.Gas);
        public string GetGasCookingFrequency() => CookingFrequency(_switchResource.CurrentUsage?.Gas);
        public string GetGasCentralHeating() => CentralHeating(_switchResource.CurrentUsage?.Gas);
        public string GetGasNumberOfOccupants() => NumberOfOccupants(_switchResource.CurrentUsage?.Gas);
        public string GetGasInsulation() => Insulation(_switchResource.CurrentUsage?.Gas);
        public string GetGasEnergyUsage() => EnergyUsage(_switchResource.CurrentUsage?.Gas);

        public string GetElectricityUsageType() => UsageType(_switchResource.CurrentUsage?.Elec);
        public string GetElectricityHouseType() => HouseType(_switchResource.CurrentUsage?.Elec);
        public string GetElectricityNumberOfBedrooms() => NumberOfBedrooms(_switchResource.CurrentUsage?.Elec);
        public string GetElectricityMainCookingSource() => MainCookingSource(_switchResource.CurrentUsage?.Elec);
        public string GetElectricityCookingFrequency() => CookingFrequency(_switchResource.CurrentUsage?.Elec);
        public string GetElectricityCentralHeating() => CentralHeating(_switchResource.CurrentUsage?.Elec);
        public string GetElectricityNumberOfOccupants() => NumberOfOccupants(_switchResource.CurrentUsage?.Elec);
        public string GetElectricityInsulation() => Insulation(_switchResource.CurrentUsage?.Elec);
        public string GetElectricityEnergyUsage() => EnergyUsage(_switchResource.CurrentUsage?.Elec);

        public string GetGasCurrentSpend() => CurrentSpend(_switchResource.CurrentUsage?.Gas);
        public string GetElectricityCurrentSpend() => CurrentSpend(_switchResource.CurrentUsage?.Elec);

        public string GetGasCurrentUsageAsKwh() => UsageAsKwh(_switchResource.CurrentUsage?.Gas);
        public string GetElectricityCurrentUsageAsKwh() => UsageAsKwh(_switchResource.CurrentUsage?.Elec);

        private static string GetUsageItem(Utility utility, int index) => utility?.UsageProfile.Usage.items.Skip(index).FirstOrDefault()?.Data;
        private static string UsageType(Utility utility) => utility?.UsageProfile.UsageType.id;
        private static string SimpleEstimate(Utility utility) => GetUsageItem(utility, 0);

        private static string HouseType(Utility utility) => GetUsageItem(utility, 0);
        private static string NumberOfBedrooms(Utility utility) => GetUsageItem(utility, 1);
        private static string MainCookingSource(Utility utility) => GetUsageItem(utility, 2);
        private static string CookingFrequency(Utility utility) => GetUsageItem(utility, 3);
        private static string CentralHeating(Utility utility) => GetUsageItem(utility, 4);
        private static string NumberOfOccupants(Utility utility) => GetUsageItem(utility, 5);
        private static string Insulation(Utility utility) => GetUsageItem(utility, 6);
        private static string EnergyUsage(Utility utility) => GetUsageItem(utility, 7);

        private static string CurrentSpend(Utility utility) => GetUsageItem(utility, 0);

        private static string UsageAsKwh(Utility utility) => GetUsageItem(utility, 0);
    }
}