using System.Threading.Tasks;
using BareboneUi.Common;
using Microsoft.AspNetCore.Mvc;

namespace BareboneUi.Pages.Preferences
{
    public class PreferencesController : Controller
    {
        private readonly ILoadModel _modelLoader;
        private readonly ISaveModel _modelSaver;

        public PreferencesController(ILoadModel modelLoader, ISaveModel modelSaver)
        {
            _modelLoader = modelLoader;
            _modelSaver = modelSaver;
        }

        public async Task<IActionResult> Index(string uri)
        {
            var model = await _modelLoader.Load<Resource, PreferencesModel>(uri);
            return View(new PreferencesViewModel
            {
                PreferencesUri = uri,
                PaymentMethods = model.GetPaymentMethods(),
                ResultsOrders = model.GetResultsOrders(),
                TariffFilterOptions = model.GetTariffFilterOptions()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(PreferencesViewModel viewModel)
        {
            var model = await _modelLoader.Load<Resource, PreferencesModel>(viewModel.PreferencesUri);

            model.Update(viewModel);

            var response = await _modelSaver.Save(model);

            return RedirectToAction("Index", "FutureSupply", new { uri = response.GetNextUrl() });
        }
    }
}