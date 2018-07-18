using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BareboneUi.Common;
using Microsoft.AspNetCore.Mvc;

namespace BareboneUi.Pages.FutureSupply
{
    public class FutureSupplyController : Controller
    {
        private readonly ILoadModel _modelLoader;
        private readonly ISaveModel _modelSaver;

        public FutureSupplyController(ILoadModel modelLoader, ISaveModel modelSaver)
        {
            _modelLoader = modelLoader;
            _modelSaver = modelSaver;
        }

        public async Task<IActionResult> Index(string uri)
        {
            var futureSupplyViewModel = await GetFutureSupplyViewModel(uri);

            return View(futureSupplyViewModel);
        }

        private async Task<FutureSupplyViewModel> GetFutureSupplyViewModel(string uri, IEnumerable<Error> errors = null)
        {
            var futureSupply = await _modelLoader.Load<Resource, FutureSupplyModel>(uri);
            var futureSupplies = await _modelLoader.Load<FutureSupplies, ResultsModel>(futureSupply.ResultsUri);

            return new FutureSupplyViewModel
            {
                DualFuelEnergySupplies = futureSupplies.GetDualFuelEnergySupplies(),
                PaymentMethods = futureSupplies.GetPaymentMethods(),
                FutureSupplyUri = uri,
                Errors = errors?.Select(x => x.Message.Text) ?? Enumerable.Empty<string>()
            }; 
        }

        [HttpPost]
        public async Task<IActionResult> Index(FutureSupplyViewModel viewModel)
        {
            var model = await _modelLoader.Load<Resource, FutureSupplyModel>(viewModel.FutureSupplyUri);

            model.Update(viewModel.FutureTariffId);

            var response = await _modelSaver.Save(model);

            if (response.Errors?.Any() ?? false)
            {
                return View(await GetFutureSupplyViewModel(viewModel.FutureSupplyUri, response.Errors));
            }

            return RedirectToAction("Index", "PrepareForTransfer", new { uri = response.GetNextUrl() });
        }
    }
}