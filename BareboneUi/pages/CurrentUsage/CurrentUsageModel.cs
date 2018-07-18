using System;
using System.Collections.Generic;
using BareboneUi.Common;

namespace BareboneUi.Pages.CurrentUsage
{
    public class CurrentUsageModel : Model
    {
        public CurrentUsageModel(Resource resource) : base(resource) { }

        public void Update(ICurrentUsageAnswer answer)
        {
            if (IsGasComparison())
            {
                GasUsageType.SetAnswer(answer.GasUsageType);
                GasSimpleEstimate.SetAnswer(answer.GasUsageSimpleEstimator);

                GasHouseType.SetAnswer(answer.HouseType);
                GasNumberOfBedrooms.SetAnswer(answer.NumberOfBedrooms);
                GasMainCookingSource.SetAnswer(answer.MainCookingSource);
                GasCookingFrequency.SetAnswer(answer.CookingFrequency);
                GasCentralHeating.SetAnswer(answer.CentralHeating);
                GasNumberOfOccupants.SetAnswer(answer.NumberOfOccupants);
                GasInsulation.SetAnswer(answer.Insulation);
                GasEnergyUsage.SetAnswer(answer.EnergyUsage);

                GasCurrentSpend.SetAnswer(answer.GasCurrentSpend);
                GasCurrentUsageAsKwh.SetAnswer(answer.GasCurrentUsageAsKwh);
            }

            if (IsElecComparison())
            {
                ElecUsageType.SetAnswer(answer.ElectricityUsageType);
                ElecSimpleEstimate.SetAnswer(answer.ElectricityUsageSimpleEstimator);

                ElectricityHouseType.SetAnswer(answer.HouseType);
                ElectricityNumberOfBedrooms.SetAnswer(answer.NumberOfBedrooms);
                ElectricityMainCookingSource.SetAnswer(answer.MainCookingSource);
                ElectricityCookingFrequency.SetAnswer(answer.CookingFrequency);
                ElectricityCentralHeating.SetAnswer(answer.CentralHeating);
                ElectricityNumberOfOccupants.SetAnswer(answer.NumberOfOccupants);
                ElectricityInsulation.SetAnswer(answer.Insulation);
                ElectricityEnergyUsage.SetAnswer(answer.EnergyUsage);

                ElectricityCurrentSpend.SetAnswer(answer.ElectricityCurrentSpend);
                ElectricityCurrentUsageAsKwh.SetAnswer(answer.ElectricityCurrentUsageAsKwh);

                if (IsEconomy7(answer))
                {
                    NightUsagePercentage.SetAnswer(new Percentage(answer.NightPercentageUsage));
                }
            }
        }

        public IEnumerable<KeyPair> GetGasUsageSimpleEstimators() => GasSimpleEstimate.DropDownValues;
        public IEnumerable<KeyPair> GetElectricityUsageSimpleEstimators() => ElecSimpleEstimate.DropDownValues;

        public IEnumerable<KeyPair> GetGasHouseTypeValues() => GasHouseType.DropDownValues;
        public IEnumerable<KeyPair> GetGasNumberOfBedroomsValues() => GasNumberOfBedrooms.DropDownValues;
        public IEnumerable<KeyPair> GetGasMainCookingSourceValues() => GasMainCookingSource.DropDownValues;
        public IEnumerable<KeyPair> GetGasCookingFrequencyValues() => GasCookingFrequency.DropDownValues;
        public IEnumerable<KeyPair> GetGasCentralHeatingValues() => GasCentralHeating.DropDownValues;
        public IEnumerable<KeyPair> GetGasNumberOfOccupantsValues() => GasNumberOfOccupants.DropDownValues;
        public IEnumerable<KeyPair> GetGasInsulationValues() => GasInsulation.DropDownValues;
        public IEnumerable<KeyPair> GetGasEnergyUsageValues() => GasEnergyUsage.DropDownValues;

        public IEnumerable<KeyPair> GetElectricityHouseTypeValues() => ElectricityHouseType.DropDownValues;
        public IEnumerable<KeyPair> GetElectricityNumberOfBedroomsValues() => ElectricityNumberOfBedrooms.DropDownValues;
        public IEnumerable<KeyPair> GetElectricityMainCookingSourceValues() => ElectricityMainCookingSource.DropDownValues;
        public IEnumerable<KeyPair> GetElectricityCookingFrequencyValues() => ElectricityCookingFrequency.DropDownValues;
        public IEnumerable<KeyPair> GetElectricityCentralHeatingValues() => ElectricityCentralHeating.DropDownValues;
        public IEnumerable<KeyPair> GetElectricityNumberOfOccupantsValues() => ElectricityNumberOfOccupants.DropDownValues;
        public IEnumerable<KeyPair> GetElectricityInsulationValues() => ElectricityInsulation.DropDownValues;
        public IEnumerable<KeyPair> GetElectricityEnergyUsageValues() => ElectricityEnergyUsage.DropDownValues;

        private bool IsElecComparison() => IsFuelIncluded("compareElec");
        private bool IsGasComparison() => IsFuelIncluded("compareGas");
        private bool IsFuelIncluded(string fuelItem) => bool.Parse(((Item)Questions["includedFuels", fuelItem]).Data);
        private static bool IsEconomy7(ICurrentUsageAnswer answer) => !string.IsNullOrEmpty(answer.NightPercentageUsage);

        private Question GasUsageType => Questions["gasUsageType", "usageType"];
        private Question GasSimpleEstimate => Questions["gasSimpleEstimate", "simpleEstimate"];

        private Question GasHouseType => Questions["gasDetailedEstimate", "houseType"];
        private Question GasNumberOfBedrooms => Questions["gasDetailedEstimate", "numberOfBedrooms"];
        private Question GasMainCookingSource => Questions["gasDetailedEstimate", "mainCookingSource"];
        private Question GasCookingFrequency => Questions["gasDetailedEstimate", "cookingFrequency"];
        private Question GasCentralHeating => Questions["gasDetailedEstimate", "centralHeating"];
        private Question GasNumberOfOccupants => Questions["gasDetailedEstimate", "numberOfOccupants"];
        private Question GasInsulation => Questions["gasDetailedEstimate", "insulation"];
        private Question GasEnergyUsage => Questions["gasDetailedEstimate", "energyUsage"];

        private Question GasCurrentSpend => Questions["gasSpend", "usageAsSpend"];
        private Question GasCurrentUsageAsKwh => Questions["gasKWhUsage", "usageAsKWh"];

        private Question ElectricityHouseType => Questions["elecDetailedEstimate", "houseType"];
        private Question ElectricityNumberOfBedrooms => Questions["elecDetailedEstimate", "numberOfBedrooms"];
        private Question ElectricityMainCookingSource => Questions["elecDetailedEstimate", "mainCookingSource"];
        private Question ElectricityCookingFrequency => Questions["elecDetailedEstimate", "cookingFrequency"];
        private Question ElectricityCentralHeating => Questions["elecDetailedEstimate", "centralHeating"];
        private Question ElectricityNumberOfOccupants => Questions["elecDetailedEstimate", "numberOfOccupants"];
        private Question ElectricityInsulation => Questions["elecDetailedEstimate", "insulation"];
        private Question ElectricityEnergyUsage => Questions["elecDetailedEstimate", "energyUsage"];

        private Question ElectricityCurrentSpend => Questions["elecSpend", "usageAsSpend"];
        private Question ElectricityCurrentUsageAsKwh => Questions["elecKWhUsage", "usageAsKWh"];

        private Question ElecUsageType => Questions["elecUsageType", "usageType"];
        private Question ElecSimpleEstimate => Questions["elecSimpleEstimate", "simpleEstimate"];
        private Question NightUsagePercentage => Questions["economy7", "nightUsagePercentage"];


        public string GetNightPercentUsage()
        {
            if (!Questions.HasItem("economy7", "nightUsagePercentage"))
            {
                return "0%";
            }

            var answer = Questions["economy7", "nightUsagePercentage"];
            var nightUsagePercentage = (int)Math.Round(decimal.Parse(answer) * 100m, 0);
            return $"{nightUsagePercentage}%";
        }
    }
}