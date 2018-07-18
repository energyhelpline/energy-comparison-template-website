using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BareboneUi.Common;
using Microsoft.AspNetCore.Mvc;

namespace BareboneUi.Pages.CurrentUsage
{
    public class CurrentUsageController : Controller
    {
        private readonly ILoadModel _modelLoader;
        private readonly ISaveModel _modelSaver;

        public CurrentUsageController(ILoadModel modelLoader, ISaveModel modelSaver)
        {
            _modelLoader = modelLoader;
            _modelSaver = modelSaver;
        }

        public async Task<IActionResult> Index(string uri)
        {
            var currentUsageViewModel = await GetCurrentUsageViewModel(uri);

            return View(currentUsageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CurrentUsageViewModel viewModel)
        {
            var model = await _modelLoader.Load<Resource, CurrentUsageModel>(viewModel.CurrentUsageUri);

            model.Update(viewModel);

            var response = await _modelSaver.Save(model);

            if (response.Errors?.Any() ?? false)
            {
                return View(await GetCurrentUsageViewModel(viewModel.CurrentUsageUri, response.Errors));
            }

            return RedirectToAction("Index", "Preferences", new { uri = response.GetNextUrl() });
        }

        private async Task<CurrentUsageViewModel> GetCurrentUsageViewModel(string uri, IEnumerable<Error> errors = null)
        {
            var model = await _modelLoader.Load<Resource, CurrentUsageModel>(uri);

            return new CurrentUsageViewModel
            {
                CurrentUsageUri = uri,
                GasUsageSimpleEstimators = model.GetGasUsageSimpleEstimators(),
                ElectricityUsageSimpleEstimators = model.GetElectricityUsageSimpleEstimators(),
                HouseTypeValues = model.GetGasHouseTypeValues(),
                NumberOfBedroomsValues = model.GetGasNumberOfBedroomsValues(),
                MainCookingSourceValues = model.GetGasMainCookingSourceValues(),
                CookingFrequencyValues = model.GetGasCookingFrequencyValues(),
                CentralHeatingValues = model.GetGasCentralHeatingValues(),
                NumberOfOccupantsValues = model.GetGasNumberOfOccupantsValues(),
                InsulationValues = model.GetGasInsulationValues(),
                EnergyUsageValues = model.GetGasEnergyUsageValues(),
                Errors = errors?.Select(error => error.Message.Text) ?? Enumerable.Empty<string>()
            };
        }
    }
}