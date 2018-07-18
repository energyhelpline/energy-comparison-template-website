using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BareboneUi.Common;
using Microsoft.AspNetCore.Mvc;

namespace BareboneUi.Pages.PrepareForTransfer
{
    public class PrepareForTransferController : Controller
    {
        private readonly ILoadModel _modelLoader;
        private readonly ISaveModel _modelSaver;

        public PrepareForTransferController(ILoadModel modelLoader, ISaveModel modelSaver)
        {
            _modelLoader = modelLoader;
            _modelSaver = modelSaver;
        }

        public IActionResult Index(string uri)
        {
            return View(GetPrepareForTransferViewModel(uri));
        }

        [HttpPost]
        public async Task<IActionResult> Index(PrepareForTransferViewModel viewModel)
        {
            var model = await _modelLoader.Load<Resource, PrepareForTransferModel>(viewModel.PrepareForTransferUri);

            model.Update(viewModel);

            var response = await _modelSaver.Save(model);

            if (response.Errors?.Any() ?? false)
            {
                return View(GetPrepareForTransferViewModel(viewModel.PrepareForTransferUri, response.Errors));
            }

            return RedirectToAction("Index", "Switch", new {uri = response.SwitchUrl, transferUri = response.GetNextUrl()});
        }

        private static PrepareForTransferViewModel GetPrepareForTransferViewModel(string uri, IEnumerable<Error> errors = null)
        {
            return new PrepareForTransferViewModel
            {
                PrepareForTransferUri = uri,
                Errors = errors?.Select(x => x.Message.Text) ?? Enumerable.Empty<string>()
            };
        }
    }
}