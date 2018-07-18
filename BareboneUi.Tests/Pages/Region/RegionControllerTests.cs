using System.Threading.Tasks;
using BareboneUi.Common;
using BareboneUi.Pages.Region;
using BareboneUi.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace BareboneUi.Tests.Pages.Region
{
    [TestFixture]
    public class RegionControllerTests
    {
        private RegionModel _model;
        private ILoadModel _loader;
        private ISaveModel _saver;
        private RegionController _controller;
        private Resource _resource;

        const string _regionUri = "region-uri";

        [SetUp]
        public void SetUp()
        {
            _resource = new ResourceBuilder()
                .WithDataTemplate()
                    .WithGroup("electricityRegion")
                        .WithItem("region")
                            .WithData("1")
                .Build();

            _model = new RegionModel(_resource);

            _loader = Substitute.For<ILoadModel>();
            _loader.Load<Resource, RegionModel>(_regionUri).Returns(_model);
            _saver = Substitute.For<ISaveModel>();

            _controller = new RegionController(_loader, _saver);
        }

        [Test]
        public async Task Adds_uri_to_view_model()
        {
            var result = (ViewResult) await _controller.Index(_regionUri);
            var viewModel = (RegionViewModel) result.Model;

            Assert.That(viewModel.RegionUri, Is.EqualTo(_regionUri));
        }

        [Test]
        public async Task Redirects_to_current_supply_when_region_is_submitted()
        {
            const string nextUrl = "current-supply-url";

            var responseStub = Substitute.For<IResponse>();
            responseStub.GetNextUrl().Returns(nextUrl);

            _saver.Save(_model).Returns(responseStub);

            var viewModel = new RegionViewModel
            {
                RegionUri = _regionUri
            };

            var result = (RedirectToActionResult)await _controller.Index(viewModel);

            Assert.That(result.ActionName, Is.EqualTo("Index").IgnoreCase);
            Assert.That(result.ControllerName, Is.EqualTo("CurrentSupply").IgnoreCase);
            Assert.That(result.RouteValues["uri"], Is.EqualTo(nextUrl));
        }

        [Test]
        public async Task Updates_region_model_when_region_is_submitted()
        {
            const string regionId = "2";

            var viewModel = new RegionViewModel
            {
                RegionId = regionId,
                RegionUri = _regionUri
            };

            await _controller.Index(viewModel);

            Assert.That(_resource.DataTemplate.GetItem("electricityRegion", "region").Data, Is.EqualTo(regionId));
        }
    }
}
