using System.Collections.Generic;
using System.Linq;
using BareboneUi.Common;
using BareboneUi.Pages.Switch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BareboneUi.Pages.CurrentSupply
{
    public class CurrentSupplyController : Controller
    {
        private readonly ILoadModel _modelLoader;
        private readonly ISaveModel _modelSaver;
        private readonly IApiClient _apiClient;

        public CurrentSupplyController(ILoadModel modelLoader, ISaveModel modelSaver, IApiClient apiClient)
        {
            _modelLoader = modelLoader;
            _modelSaver = modelSaver;
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index(string uri)
        {
            var currentSupplyViewModel = await GetCurrentSupplyViewModel(uri);

            return View(currentSupplyViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CurrentSupplyViewModel viewModel)
        {
            var model = await _modelLoader.Load<Resource, CurrentSupplyModel>(viewModel.CurrentSupplyUrl);

            model.Update(viewModel);

            var response = await _modelSaver.Save(model);

            if (response.Errors?.Any() ?? false)
            {
                return View(await GetCurrentSupplyViewModel(viewModel.CurrentSupplyUrl, response.Errors));
            }

            var switchResource = await _modelLoader.Load<SwitchResource, SwitchModel>(response.SwitchUrl);

            return switchResource.IsProRata
                ? RedirectToAction("Index", "ContractExpiryDate", new { uri = switchResource.ContractExpiryDateUri })
                : RedirectToAction("Index", "CurrentUsage", new { uri = response.GetNextUrl() });
        }

        private async Task<CurrentSupplyViewModel> GetCurrentSupplyViewModel(string uri, IEnumerable<Error> errors = null)
        {
            var currentSupplyModel = await _modelLoader.Load<Resource, CurrentSupplyModel>(uri);
            var currentSuppliesResource = await _apiClient.GetAsync(currentSupplyModel.GetCurrentSuppliesUri());

            return new CurrentSupplyViewModel
            {
                CurrentSupplyUrl = uri,
                CurrentSupplies = currentSuppliesResource,
                Errors = errors?.Select(x => x.Message.Text) ?? Enumerable.Empty<string>()
            };
        }
    }
}