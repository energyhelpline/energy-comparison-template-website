using System.Linq;
using BareboneUi.Pages.CurrentUsage;

namespace BareboneUi.Pages.Switch
{
    public class CurrentUsageEstimatorValueMap
    {
        private readonly SwitchModel _switchModel;
        private readonly CurrentUsageModel _usageModel;

        public CurrentUsageEstimatorValueMap(SwitchModel switchModel, CurrentUsageModel usageModel)
        {
            _switchModel = switchModel;
            _usageModel = usageModel;
        }

        public string GasSimpleEstimator() => _usageModel.GetGasUsageSimpleEstimators().SingleOrDefault(x => x.Id == _switchModel.GetGasUsageSimpleEstimator())?.Name;
        public string ElectricitySimpleEstimator() => _usageModel.GetElectricityUsageSimpleEstimators().SingleOrDefault(x => x.Id == _switchModel.ElectricityUsageSimpleEstimator)?.Name;

        public string GasHouseTypeName() => _usageModel.GetGasHouseTypeValues().SingleOrDefault(x => x.Id == _switchModel.GetGasHouseType())?.Name;
        public string GasNumberOfBedrooms() => _usageModel.GetGasNumberOfBedroomsValues().SingleOrDefault(x => x.Id == _switchModel.GetGasNumberOfBedrooms())?.Name;
        public string GasMainCookingSource() => _usageModel.GetGasMainCookingSourceValues().SingleOrDefault(x => x.Id == _switchModel.GetGasMainCookingSource())?.Name;
        public string GasCookingFrequency() => _usageModel.GetGasCookingFrequencyValues().SingleOrDefault(x => x.Id == _switchModel.GetGasCookingFrequency())?.Name;
        public string GasCentralHeating() => _usageModel.GetGasCentralHeatingValues().SingleOrDefault(x => x.Id == _switchModel.GetGasCentralHeating())?.Name;
        public string GasNumberOfOccupants() => _usageModel.GetGasNumberOfOccupantsValues().SingleOrDefault(x => x.Id == _switchModel.GetGasNumberOfOccupants())?.Name;
        public string GasInsulation() => _usageModel.GetGasInsulationValues().SingleOrDefault(x => x.Id == _switchModel.GetGasInsulation())?.Name;
        public string GasEnergyUsage() => _usageModel.GetGasEnergyUsageValues().SingleOrDefault(x => x.Id == _switchModel.GetGasEnergyUsage())?.Name;

        public string ElectricityHouseTypeName() => _usageModel.GetElectricityHouseTypeValues().SingleOrDefault(x => x.Id == _switchModel.GetElectricityHouseType())?.Name;
        public string ElectricityNumberOfBedrooms() => _usageModel.GetElectricityNumberOfBedroomsValues().SingleOrDefault(x => x.Id == _switchModel.GetElectricityNumberOfBedrooms())?.Name;
        public string ElectricityMainCookingSource() => _usageModel.GetElectricityMainCookingSourceValues().SingleOrDefault(x => x.Id == _switchModel.GetElectricityMainCookingSource())?.Name;
        public string ElectricityCookingFrequency() => _usageModel.GetElectricityCookingFrequencyValues().SingleOrDefault(x => x.Id == _switchModel.GetElectricityCookingFrequency())?.Name;
        public string ElectricityCentralHeating() => _usageModel.GetElectricityCentralHeatingValues().SingleOrDefault(x => x.Id == _switchModel.GetElectricityCentralHeating())?.Name;
        public string ElectricityNumberOfOccupants() => _usageModel.GetElectricityNumberOfOccupantsValues().SingleOrDefault(x => x.Id == _switchModel.GetElectricityNumberOfOccupants())?.Name;
        public string ElectricityInsulation() => _usageModel.GetElectricityInsulationValues().SingleOrDefault(x => x.Id == _switchModel.GetElectricityInsulation())?.Name;
        public string ElectricityEnergyUsage() => _usageModel.GetElectricityEnergyUsageValues().SingleOrDefault(x => x.Id == _switchModel.GetElectricityEnergyUsage())?.Name;
    }
}