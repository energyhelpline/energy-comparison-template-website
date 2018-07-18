using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BareboneUi.Common;
using BareboneUi.Pages.CurrentSupply;
using BareboneUi.Pages.Switch;
using BareboneUi.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace BareboneUi.Tests.Pages.CurrentSupply
{
    [TestFixture]
    public class CurrentSupplyControllerTests
    {
        private const string Uri = "current-supply-uri";
        private const string CurrentSuppliesUri = "current-supplies-uri";

        private CurrentSupplyController _sut;
        private ILoadModel _modelLoader;
        private ISaveModel _modelSaver;
        private ResourceBuilder _resourceBuilder;
        private CurrentSupplyModel _currentSupplyModel;
        private IApiClient _apiClient;
        private string _currentSuppliesResourceJson;

        [SetUp]
        public void SetUp()
        {
            _modelLoader = Substitute.For<ILoadModel>();
            _modelSaver = Substitute.For<ISaveModel>();
            _apiClient = Substitute.For<IApiClient>();

            _sut = new CurrentSupplyController(_modelLoader, _modelSaver, _apiClient);
            _currentSuppliesResourceJson = GetCurrentSuppliesJson();

            _resourceBuilder = new ResourceBuilder()
                .WithDataTemplate()
                    .WithGroup("includedFuels")
                        .WithItem("compareGas")
                        .WithItem("compareElec")
                    .WithGroup("gasTariff")
                        .WithItem("supplier")
                        .WithItem("supplierTariff")
                        .WithItem("paymentMethod")
                    .WithGroup("elecTariff")
                        .WithItem("supplier")
                        .WithItem("supplierTariff")
                        .WithItem("paymentMethod")
                        .WithItem("economy7")
                .WithLinkedData("/rels/domestic/current-supplies", CurrentSuppliesUri);
        }

        [Test]
        public async Task Adds_query_string_uri_to_current_supply_view_viewModel()
        {
            var viewModel = await GetDefaultCurrentSupplyViewModel();

            Assert.That(viewModel.CurrentSupplyUrl, Is.EqualTo(Uri));
        }

        [Test]
        public async Task Populates_current_supplies()
        {
            var viewModel = await GetDefaultCurrentSupplyViewModel();

            Assert.That(viewModel.CurrentSupplies, Is.EqualTo(_currentSuppliesResourceJson));
        }

        [Test]
        public async Task Post_with_any_errors_returns_errors()
        {
            _resourceBuilder.WithError("some-error");

            StubModelLoader();

            _modelSaver
                .Save(_currentSupplyModel)
                .Returns(new Response(new ResourceBuilder().WithError("some-error").Build()));

            var requestViewModel = new CurrentSupplyViewModel {CurrentSupplyUrl = Uri};
            var result = (ViewResult)await _sut.Index(requestViewModel);
            var responseViewModel = (CurrentSupplyViewModel)result.Model;

            var error = responseViewModel.Errors.Single();
            Assert.That(error, Is.EqualTo("some-error"));
        }

        [Test]
        public async Task Updates_the_model()
        {
            var resource = _resourceBuilder.Build();
            StubModelLoader();
            var response = Substitute.For<IResponse>();

            _modelSaver.Save(_currentSupplyModel).Returns(response);

            SetupStubbedSwitch();

            response.SwitchUrl.Returns("switch-uri");

            var viewModel = new CurrentSupplyViewModel
            {
                CurrentSupplyUrl = Uri,
                IsElectricityComparison = true,
                IsGasComparison = false
            };

            await _sut.Index(viewModel);

            Assert.That(resource.DataTemplate.GetItem("includedFuels", "compareGas").Data, Is.EqualTo(false.ToString()));
            Assert.That(resource.DataTemplate.GetItem("includedFuels", "compareElec").Data, Is.EqualTo(true.ToString()));
        }

        [TestCase(true, "ContractExpiryDate", "contract-expiry-date-uri")]
        [TestCase(false, "CurrentUsage", "next-uri")]
        public async Task Redirects_to_page(bool isProRata, string expectedControllerName, string expectedUri)
        {
            StubModelLoader();
            var response = Substitute.For<IResponse>();

            _modelSaver.Save(_currentSupplyModel).Returns(response);

            response.SwitchUrl.Returns("switch-uri");
            response.GetNextUrl().Returns("next-uri");

            SetupStubbedSwitch(isProRata);

            var viewModel = new CurrentSupplyViewModel
            {
                CurrentSupplyUrl = Uri
            };

            var result = (RedirectToActionResult)await _sut.Index(viewModel);

            Assert.That(result.ActionName, Is.EqualTo("Index"));
            Assert.That(result.ControllerName, Is.EqualTo(expectedControllerName));
            Assert.That(result.RouteValues["uri"], Is.EqualTo(expectedUri));
        }

        private void SetupStubbedSwitch(bool isProRata = false)
        {
            var switchResource = new SwitchResource
            {
                Links = new List<Link>
                {
                    new Link
                    {
                        Rel = isProRata ? "/rels/domestic/contract-details" : "dummy-rel",
                        Uri = isProRata ? "contract-expiry-date-uri" : "dummy-uri"
                    },
                    new Link
                    {
                        Rel = "/rels/next",
                        Uri = "next-uri"
                    }
                }
            };

            var switchModel = new SwitchModel(switchResource);
            _modelLoader.Load<SwitchResource, SwitchModel>("switch-uri").Returns(switchModel);
        }

        private async Task<CurrentSupplyViewModel> GetDefaultCurrentSupplyViewModel()
        {
            StubModelLoader();

            var result = (ViewResult)await _sut.Index(Uri);
            var viewModel = (CurrentSupplyViewModel)result.Model;
            return viewModel;
        }

        private void StubModelLoader()
        {
            _currentSupplyModel = new CurrentSupplyModel(_resourceBuilder.Build());
            _modelLoader.Load<Resource, CurrentSupplyModel>(Uri).Returns(_currentSupplyModel);

            _apiClient.GetAsync(CurrentSuppliesUri).Returns(_currentSuppliesResourceJson);
        }
        private string GetCurrentSuppliesJson()
        {
            return @"
{
    ""fuels"": {
        ""electricity"": {
            ""defaultSupplier"": null,
            ""suppliers"": [
                {
                    ""id"": ""26"",
                    ""name"": ""Gas supplier 3"",
                    ""defaultSupplierTariff"": null,
                    ""supplierTariffs"": [
                        {
                            ""id"": ""13"",
                            ""name"": ""Gas Supplier 3 Tariff"",
                            ""paymentMethods"": [
                                {
                                    ""id"": ""67"",
                                    ""name"": ""Gas Supplier 3 Tariff Payment Method""
                                }
                            ]
                        }
                    ]
                },
                {
                    ""id"": ""256"",
                    ""name"": ""Elec supplier 3"",
                    ""defaultSupplierTariff"": null,
                    ""supplierTariffs"": [
                        {
                            ""id"": ""131"",
                            ""name"": ""Elec supplier 3 tariff"",
                            ""paymentMethods"": [
                                {
                                    ""id"": ""676"",
                                    ""name"": ""Elec supplier 3 tariff payment method""
                                }
                            ]
                        }
                    ]
                }
            ]
        },
        ""gas"": {
            ""defaultSupplier"": null,
            ""suppliers"": [
                {
                    ""id"": ""26"",
                    ""name"": ""Gas supplier 3"",
                    ""defaultSupplierTariff"": null,
                    ""supplierTariffs"": [
                        {
                            ""id"": ""13"",
                            ""name"": ""Gas Supplier 3 Tariff"",
                            ""paymentMethods"": [
                                {
                                    ""id"": ""67"",
                                    ""name"": ""Gas Supplier 3 Tariff Payment Method""
                                }
                            ]
                        }
                    ]
                },
                {
                    ""id"": ""256"",
                    ""name"": ""Elec supplier 3"",
                    ""defaultSupplierTariff"": null,
                    ""supplierTariffs"": [
                        {
                            ""id"": ""131"",
                            ""name"": ""Elec supplier 3 tariff"",
                            ""paymentMethods"": [
                                {
                                    ""id"": ""676"",
                                    ""name"": ""Elec supplier 3 tariff payment method""
                                }
                            ]
                        }
                    ]
                }
            ]
        }
    },
    ""paymentMethods"": [
        {
            ""id"": ""67"",
            ""name"": ""Gas Supplier 3 Tariff Payment Method""
        },
        {
            ""id"": ""676"",
            ""name"": ""Elec supplier 3 tariff payment method""
        }
    ]
}
";
        }
    }
}