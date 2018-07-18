using System.Linq;
using System.Threading.Tasks;
using BareboneUi.Common;
using BareboneUi.Pages.PrepareForTransfer;
using BareboneUi.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace BareboneUi.Tests.Pages.PrepareForTransfer
{
    public class PrepareForTransferControllerTests
    {
        private ILoadModel _modelLoader;
        private ISaveModel _modelSaver;
        private PrepareForTransferController _sut;
        private PrepareForTransferViewModel _viewModel;
        private Resource _resource;
        private ResourceBuilder _resourceBuilder;
        private PrepareForTransferModel _model;

        [SetUp]
        public void SetUp()
        {
            _modelLoader = Substitute.For<ILoadModel>();
            _modelSaver = Substitute.For<ISaveModel>();

            _viewModel = new PrepareForTransferViewModel
            {
                PrepareForTransferUri = "prepare-for-transfer-uri",
                CallbackUri = "callback url",
                ThankYouUri = "thankyou url"
            };

            _resourceBuilder = new ResourceBuilder()
                .WithDataTemplate()
                    .WithGroup("returnURLs")
                        .WithItem("thankYou")
                        .WithItem("callback");

            _sut = new PrepareForTransferController(_modelLoader, _modelSaver);
        }

        [Test]
        public void Adds_query_string_uri_to_prepare_to_transfer_view_model()
        {
            StubModelLoader();

            const string prepareForTransferUri = "prepare-for-transfer-uri";

            var result = (ViewResult) _sut.Index(prepareForTransferUri);
            var model = (PrepareForTransferViewModel) result.Model;

            Assert.That(model.PrepareForTransferUri, Is.EqualTo(prepareForTransferUri));
        }

        [Test]
        public async Task Returns_prepare_for_transfer_uri_when_post_to_prepare_for_transfer_resource()
        {
            StubModelLoader();

            const string switchUrl = "switch-url";
            const string transferUrl = "transfer-url";

            var response = Substitute.For<IResponse>();
            response.GetNextUrl().Returns(transferUrl);
            response.SwitchUrl.Returns(switchUrl);

            _modelSaver.Save(_model).Returns(response);

            var result = (RedirectToActionResult) await _sut.Index(_viewModel);

            Assert.That(result.ControllerName, Is.EqualTo("Switch"));
            Assert.That(result.ActionName, Is.EqualTo("Index"));
            Assert.That(result.RouteValues["uri"], Is.EqualTo(switchUrl));
            Assert.That(result.RouteValues["transferUri"], Is.EqualTo(transferUrl));
        }

        [Test]
        public async Task Updates_prepare_for_trasfer_model_when_post_to_prepare_for_transfer()
        {
            StubModelLoader();

            await _sut.Index(_viewModel);

            Assert.That(_resource.DataTemplate.GetItem("returnURLs", "thankYou").Data, Is.EqualTo(_viewModel.ThankYouUri));
            Assert.That(_resource.DataTemplate.GetItem("returnURLs", "callback").Data, Is.EqualTo(_viewModel.CallbackUri));
        }

        [Test]
        public async Task Post_with_any_errors_returns_errors()
        {
            _resourceBuilder.WithError("some-prepare-for-transfer-error");

            StubModelLoader();

            _modelSaver
                .Save(_model)
                .Returns(new Response(_resource));

            var result = (ViewResult)await _sut.Index(_viewModel);
            var responseViewModel = (PrepareForTransferViewModel)result.Model;

            var error = responseViewModel.Errors.Single();
            Assert.That(error, Is.EqualTo("some-prepare-for-transfer-error"));
        }

        private void StubModelLoader()
        {
            _resource = _resourceBuilder.Build();
            _model = new PrepareForTransferModel(_resource);
            _modelLoader.Load<Resource, PrepareForTransferModel>(_viewModel.PrepareForTransferUri).Returns(_model);
        }
    }
}
