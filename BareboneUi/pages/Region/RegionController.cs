using System.Threading.Tasks;
using BareboneUi.Common;
using Microsoft.AspNetCore.Mvc;

namespace BareboneUi.Pages.Region
{
    public class RegionController : Controller
    {
        private readonly ILoadModel _loader;
        private readonly ISaveModel _saver;

        public RegionController(ILoadModel loader, ISaveModel saver)
        {
            _loader = loader;
            _saver = saver;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string uri)
        {
            var regionResource = await _loader.Load<Resource, RegionModel>(uri);
            var regions = regionResource.GetRegions();

            return View(new RegionViewModel
            {
                Regions = regions,
                RegionUri = uri
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegionViewModel viewModel)
        {
            var region = await _loader.Load<Resource, RegionModel>(viewModel.RegionUri);

            region.Update(viewModel);

            var response = await _saver.Save(region);
            return RedirectToAction("Index", "CurrentSupply", new { uri = response.GetNextUrl() });
        }
    }
}