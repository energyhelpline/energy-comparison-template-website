using System.Threading.Tasks;
using BareboneUi.Common;
using Microsoft.AspNetCore.Mvc;

namespace BareboneUi.Pages.FutureTariffDetails
{
    public class FutureTariffDetailsController : Controller
    {
        private readonly ILoadModel _modelLoader;

        public FutureTariffDetailsController(ILoadModel modelLoader)
        {
            _modelLoader = modelLoader;
        }

        public async Task<IActionResult> Index(string uri)
        {
            var model = await _modelLoader.Load<FutureTariffDetail, FutureTariffDetailsModel>(uri);
            return View(new FutureTariffDetailsViewModel
            {
                Supplies = model.GetSupplies()
            });
        }
    }
}
