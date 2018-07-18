using System.Linq;
using System.Threading.Tasks;
using BareboneUi.Common;
using BareboneUi.Pages.Home;
using BareboneUi.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace BareboneUi.Tests.Pages.Home
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private StartSwitchModel _startSwitch;
        private ISaveModel _modelSaver;
        private Resource _startSwitchResource;

        [SetUp]
        public void SetUp()
        {
            var entry = new EntryModel(
                new ResourceBuilder()
                    .WithLink("/rels/domestic/switches /rels/bookmark", "start-switch-uri")
                    .Build());

            _startSwitchResource = new ResourceBuilder()
                .WithDataTemplate()
                    .WithGroup("supplyPostcode")
                        .WithItem("postcode")
                    .WithGroup("references")
                        .WithItem("partnerReference")
                        .WithItem("apiKey")
                .Build();

            var configuration = Substitute.For<IConfiguration>();
            configuration.GetValue<string>("ApiEntryPoint").Returns("test-entry-point");

            _startSwitch = new StartSwitchModel(_startSwitchResource, configuration);
            _modelSaver = Substitute.For<ISaveModel>();

            var modelLoader = Substitute.For<ILoadModel>();
            modelLoader.Load<Resource, EntryModel>("test-entry-point").Returns(entry);
            modelLoader.Load<Resource, StartSwitchModel>("start-switch-uri").Returns(_startSwitch);

            _controller = new HomeController(modelLoader, _modelSaver, configuration);
        }

        [Test]
        public void Get_the_view_for_start_switch()
        {
            var result = _controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Post_with_missing_postcode_returns_errors()
        {
            _modelSaver
                .Save(_startSwitch)
                .Returns(new Response(new ResourceBuilder().WithError("some-error").Build()));

            var requestViewModel = new StartSwitchViewModel();
            var result = (ViewResult) await _controller.Index(requestViewModel);
            var responseViewModel = (StartSwitchViewModel)result.Model;

            var error = responseViewModel.Errors.Single();
            Assert.That(error, Is.EqualTo("some-error"));
        }

        [Test]
        public async Task Post_with_known_postcode_redirects_to_current_supply()
        {
            const string postcode = "post-code";

            _modelSaver
                .Save(_startSwitch)
                .Returns(new Response(new ResourceBuilder().WithLink("/rels/next", "next-uri").Build()));

            var viewModel = new StartSwitchViewModel { Postcode = postcode };
            var result = (RedirectToActionResult) await _controller.Index(viewModel);

            Assert.That(result.ActionName, Is.EqualTo("index").IgnoreCase);
            Assert.That(result.ControllerName, Is.EqualTo("currentSupply").IgnoreCase);
            Assert.That(result.RouteValues["uri"] , Is.EqualTo("next-uri"));
        }

        [Test]
        public async Task Post_with_unknown_postcode_redirects_to_region()
        {
            const string nextUri = "xxxxx region xxxxx";

            _modelSaver
                .Save(_startSwitch)
                .Returns(new Response(new ResourceBuilder().WithLink("/rels/next /rels/domestic/region", nextUri).Build()));

            var viewModel = new StartSwitchViewModel();
            var result = (RedirectToActionResult) await _controller.Index(viewModel);

            Assert.That(result.ActionName, Is.EqualTo("Index").IgnoreCase);
            Assert.That(result.ControllerName, Is.EqualTo("Region").IgnoreCase);
            Assert.That(result.RouteValues["uri"], Is.EqualTo(nextUri));
        }

        [Test]
        public async Task Post_updates_the_postcode_in_start_switch_model()
        {
            const string postcode = "post-code";

            _modelSaver
                .Save(_startSwitch)
                .Returns(new Response(new ResourceBuilder().WithLink("/rels/next", "next-uri").Build()));

            var viewModel = new StartSwitchViewModel { Postcode = postcode };
            await _controller.Index(viewModel);

            Assert.That(_startSwitchResource.DataTemplate.GetItem("supplyPostcode", "postcode").Data, Is.EqualTo("post-code"));
        }
    }
}