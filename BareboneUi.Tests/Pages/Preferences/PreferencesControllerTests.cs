using System.Collections.Generic;
using System.Threading.Tasks;
using BareboneUi.Common;
using BareboneUi.Pages.Preferences;
using BareboneUi.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace BareboneUi.Tests.Pages.Preferences
{
    [TestFixture]
    public class PreferencesControllerTests
    {
        private const string _url = "preferences-uri";
        private const string _paymentMethod = "payment method";
        private const string _resultOrders = "result orders";
        private const string _tariffFilterOption = "tariff filter option";

        private PreferencesController _sut;
        private ILoadModel _modelLoader;
        private ISaveModel _modelSaver;
        private Resource _resource;
        private PreferencesViewModel _viewModel;
        private PreferencesModel _model;

        [SetUp]
        public void SetUp()
        {
            var response = Substitute.For<IResponse>();
            response.GetNextUrl().Returns("next-url");

            _modelLoader = Substitute.For<ILoadModel>();
            _modelSaver = Substitute.For<ISaveModel>();

            _resource = new ResourceBuilder()
                .WithDataTemplate()
                    .WithGroup("tariffFilterOptions")
                        .WithItem("tariffFilterOption")
                    .WithGroup("resultsOrder")
                        .WithItem("resultsOrder")
                    .WithGroup("limitToPaymentType")
                        .WithItem("paymentMethod")
                .Build();

            _viewModel = new PreferencesViewModel
            {
                PreferencesUri = _url,
                PaymentMethod = _paymentMethod,
                ResultsOrder = _resultOrders,
                TariffFilterOption = _tariffFilterOption
            };

            _model = new PreferencesModel(_resource);
            _modelLoader.Load<Resource, PreferencesModel>(_viewModel.PreferencesUri).Returns(_model);
            _modelSaver.Save(_model).Returns(response);

            _sut = new PreferencesController(_modelLoader, _modelSaver);
        }

        [Test]
        public async Task Adds_query_string_uri_to_current_usage_view_model()
        {
            var viewModel = await GetDefaultPreferencesViewModel();

            Assert.That(viewModel.PreferencesUri, Is.EqualTo(_url));
        }

        [Test]
        public async Task Populates_preferences_view_model_payment_methods_with_acceptable_values()
        {
            var acceptableValues = StubAcceptableValues();
            _resource.DataTemplate.GetItem("limitToPaymentType", "paymentMethod").AcceptableValues = acceptableValues;

            var viewModel = await GetDefaultPreferencesViewModel();

            Assert.That(viewModel.PaymentMethods, Is.EquivalentTo(acceptableValues));
        }

        [Test]
        public async Task Populates_preferences_view_model_result_orders_with_acceptable_values()
        {
            var acceptableValues = StubAcceptableValues();
            _resource.DataTemplate.GetItem("resultsOrder", "resultsOrder").AcceptableValues = acceptableValues;

            var viewModel = await GetDefaultPreferencesViewModel();

            Assert.That(viewModel.ResultsOrders, Is.EquivalentTo(acceptableValues));
        }

        [Test]
        public async Task Populates_preferences_view_model_tariff_filter_options_with_acceptable_values()
        {
            var acceptableValues = StubAcceptableValues();
            _resource.DataTemplate.GetItem("tariffFilterOptions", "tariffFilterOption").AcceptableValues = acceptableValues;

            var viewModel = await GetDefaultPreferencesViewModel();

            Assert.That(viewModel.TariffFilterOptions, Is.EquivalentTo(acceptableValues));
        }

        [Test]
        public async Task Redirects_to_future_supply_when_post_to_preferences_resource()
        {
            var result = (RedirectToActionResult) await _sut.Index(_viewModel);

            Assert.That(result.ActionName, Is.EqualTo("Index"));
            Assert.That(result.ControllerName, Is.EqualTo("FutureSupply"));
            Assert.That(result.RouteValues["uri"], Is.EqualTo("next-url"));
        }

        [Test]
        public async Task Updates_preferences_model_when_preferences_is_submitted()
        {
            await _sut.Index(_viewModel);

            Assert.That(_resource.DataTemplate.GetItem("limitToPaymentType", "paymentMethod").Data, Is.EqualTo(_paymentMethod));
            Assert.That(_resource.DataTemplate.GetItem("resultsOrder", "resultsOrder").Data, Is.EqualTo(_resultOrders));
            Assert.That(_resource.DataTemplate.GetItem("tariffFilterOptions", "tariffFilterOption").Data, Is.EqualTo(_tariffFilterOption));
        }

        private async Task<PreferencesViewModel> GetDefaultPreferencesViewModel()
        {
            var result = (ViewResult) await _sut.Index(_url);
            return (PreferencesViewModel) result.Model;
        }

        private static List<KeyPair> StubAcceptableValues()
        {
            return new List<KeyPair>
            {
                new KeyPair{ Id = "id1", Name = "value1" },
                new KeyPair{ Id = "id2", Name = "value2" },
            };
        }
    }
}
