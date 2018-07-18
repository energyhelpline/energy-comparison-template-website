using System.Threading.Tasks;
using BareboneUi.Common;
using Microsoft.AspNetCore.Mvc;

namespace BareboneUi.Pages.ContractExpiryDate
{
    public class ContractExpiryDateController : Controller
    {
        private readonly ILoadModel _modelLoader;
        private readonly ISaveModel _modelSaver;

        public ContractExpiryDateController(ILoadModel modelLoader, ISaveModel modelSaver)
        {
            _modelLoader = modelLoader;
            _modelSaver = modelSaver;
        }

        public IActionResult Index(string uri)
        {
            return View(new ContractExpiryDateViewModel { ContractExpiryDateUri = uri });
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContractExpiryDateViewModel model)
        {
            var contractExpiry = await _modelLoader.Load<Resource, ContractExpiryDateModel>(model.ContractExpiryDateUri);

            contractExpiry.Update(model);

            var response = await _modelSaver.Save(contractExpiry);

            return RedirectToAction("Index", "CurrentUsage", new { uri = response.GetNextUrl() });
        }
    }
}