using System.Linq;
using System.Threading.Tasks;
using BareboneUi.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BareboneUi.Pages.Home
{
    public class HomeController : Controller
    {
        private readonly ISaveModel _modelSaver;
        private readonly ILoadModel _modelLoader;
        private readonly string _apiEntryPoint;

        public HomeController(ILoadModel modelLoader, ISaveModel modelSaver, IConfiguration configuration)
        {
            _modelSaver = modelSaver;
            _modelLoader = modelLoader;
            _apiEntryPoint = configuration.GetValue<string>("ApiEntryPoint");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new StartSwitchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(StartSwitchViewModel viewModel)
        {
            var entry = await _modelLoader.Load<Resource, EntryModel>(_apiEntryPoint);
            var startSwitch = await _modelLoader.Load<Resource, StartSwitchModel>(entry.StartSwitchUrl);

            startSwitch.Update(viewModel.Postcode);

            var response = await _modelSaver.Save(startSwitch);

            if (response.Errors?.Any() ?? false)
            {
                return View(new StartSwitchViewModel
                {
                    Errors = response.Errors.Select(x => x.Message.Text)
                });
            }

            var redirectController = response.ContainsRel("/rels/domestic/region") ? "Region" : "CurrentSupply";

            return RedirectToAction("Index", redirectController, new { uri = response.GetNextUrl() });
        }
    }
}