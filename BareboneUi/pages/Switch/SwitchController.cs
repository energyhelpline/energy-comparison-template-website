using System.Threading.Tasks;
using BareboneUi.Common;
using BareboneUi.Pages.CurrentUsage;
using Microsoft.AspNetCore.Mvc;

namespace BareboneUi.Pages.Switch
{
    public class SwitchController : Controller
    {
        private readonly ILoadModel _modelLoader;

        public SwitchController(ILoadModel modelLoader)
        {
            _modelLoader = modelLoader;
        }

        public async Task<IActionResult> Index(string uri, string transferUri)
        {
            var switchResource =await _modelLoader.Load<SwitchResource, SwitchModel>(uri);
            var usageModel = await _modelLoader.Load<Resource, CurrentUsageModel>(switchResource.CurrentUsageUri);

            var currentUsageEstimatorValueMap = new CurrentUsageEstimatorValueMap(switchResource, usageModel);
            var viewModel = new SwitchViewModel
            {
                Postcode = switchResource.Postcode,
                GasContractExpiryDate = switchResource.GasContractExpiryDate?.ToString("dd/MM/yyyy"),
                GasPaymentMethod = switchResource.GasPaymentMethod,
                GasSupplyName = switchResource.GasSupplyName,
                GasTariff = switchResource.GasTariff,
                ElectricityContractExpiryDate = switchResource.ElectricityContractExpiryDate?.ToString("dd/MM/yyyy"),
                ElectricityPaymentMethod = switchResource.ElectricityPaymentMethod,
                ElectricitySupplyName = switchResource.ElectricitySupplyName,
                ElectricityTariff = switchResource.ElectricityTariff,
                ElectricityEcomony7 = switchResource.ElectricityEcomony7.ToString(),
                NightPercentUsage = usageModel.GetNightPercentUsage(),
                GasUsageType = switchResource.GetGasUsageType(),
                ElectricityUsageType = switchResource.GetElectricityUsageType(),
                GasUsageSimpleEstimator = currentUsageEstimatorValueMap.GasSimpleEstimator(),
                ElectricityUsageSimpleEstimator = currentUsageEstimatorValueMap.ElectricitySimpleEstimator(),
                GasHouseType = currentUsageEstimatorValueMap.GasHouseTypeName(),
                GasNumberOfBedrooms = currentUsageEstimatorValueMap.GasNumberOfBedrooms(),
                GasMainCookingSource = currentUsageEstimatorValueMap.GasMainCookingSource(),
                GasCookingFrequency = currentUsageEstimatorValueMap.GasCookingFrequency(),
                GasCentralHeating = currentUsageEstimatorValueMap.GasCentralHeating(),
                GasNumberOfOccupants = currentUsageEstimatorValueMap.GasNumberOfOccupants(),
                GasInsulation = currentUsageEstimatorValueMap.GasInsulation(),
                GasEnergyUsage = currentUsageEstimatorValueMap.GasEnergyUsage(),
                ElectricityHouseType = currentUsageEstimatorValueMap.ElectricityHouseTypeName(),
                ElectricityNumberOfBedrooms = currentUsageEstimatorValueMap.ElectricityNumberOfBedrooms(),
                ElectricityMainCookingSource = currentUsageEstimatorValueMap.ElectricityMainCookingSource(),
                ElectricityCookingFrequency = currentUsageEstimatorValueMap.ElectricityCookingFrequency(),
                ElectricityCentralHeating = currentUsageEstimatorValueMap.ElectricityCentralHeating(),
                ElectricityNumberOfOccupants = currentUsageEstimatorValueMap.ElectricityNumberOfOccupants(),
                ElectricityInsulation = currentUsageEstimatorValueMap.ElectricityInsulation(),
                ElectricityEnergyUsage = currentUsageEstimatorValueMap.ElectricityEnergyUsage(),
                GasCurrentSpend = switchResource.GetGasCurrentSpend(),
                ElectricityCurrentSpend = switchResource.GetElectricityCurrentSpend(),
                GasCurrentUsageAsKwh = switchResource.GetGasCurrentUsageAsKwh(),
                ElectricityCurrentUsageAsKwh = switchResource.GetElectricityCurrentUsageAsKwh(),
                TransferUrl = transferUri
            };

            return View(viewModel);
        }
    }
}