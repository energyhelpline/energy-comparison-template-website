using BareboneUi.Common;
using BareboneUi.Pages.FutureSupply;
using BareboneUi.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace BareboneUi.Tests.Pages.FutureSupply
{
    public class FutureSupplyControllerTests
    {
        private const string _uri = "future-supply-uri";
        private const string _futureSuppliesUri = "future-supplies-uri";

        private FutureSupplyController _sut;
        private ILoadModel _modelLoader;
        private ISaveModel _modelSaver;
        private FutureSuppliesResourceBuilder _futureSuppliesResourceBuilder;
        private ResourceBuilder _futureSupplyResourceBuilder;
        private FutureSupplyModel _futureSupplyModel;
        private Resource _futureSupplyResource;

        [SetUp]
        public void SetUp()
        {
            _modelLoader = Substitute.For<ILoadModel>();
            _modelSaver = Substitute.For<ISaveModel>();

            _futureSuppliesResourceBuilder = new FutureSuppliesResourceBuilder();

            _futureSupplyResourceBuilder = new ResourceBuilder()
                .WithLinkedData("/rels/domestic/future-supplies", _futureSuppliesUri);

            _sut = new FutureSupplyController(_modelLoader, _modelSaver);
        }

        [Test]
        public async Task Adds_query_string_uri_to_future_supply_view_model()
        {
            _futureSuppliesResourceBuilder.WithResult(new Result
            {
                SupplyType = new SupplyType
                {
                    Id = "4"
                },
                EnergySupplies = new EnergySupply[] { }
            });
            StubFutureSupplies();
            StubFutureSupply();

            var result = (ViewResult)await _sut.Index(_uri);
            var model = (FutureSupplyViewModel)result.Model;

            Assert.That(model.FutureSupplyUri, Is.EqualTo(_uri));
        }

        [Test]
        public async Task Populates_future_supply_view_model_dual_fuel_energy_suppliers_with_acceptable_values()
        {
            var result1 = new Result
            {
                SupplyType = new SupplyType
                {
                    Id = "4"
                },
                EnergySupplies = new[]
                {
                    new EnergySupply(),
                    new EnergySupply()
                }
            };
            var result2 = new Result
            {
                SupplyType = new SupplyType
                {
                    Id = "other result"
                },
                EnergySupplies = new[]
                {
                    new EnergySupply()
                }
            };
            _futureSuppliesResourceBuilder
                .WithResult(result1)
                .WithResult(result2);
            StubFutureSupplies();
            StubFutureSupply();

            var result = (ViewResult)await _sut.Index(_uri);
            var model = (FutureSupplyViewModel)result.Model;

            Assert.That(model.DualFuelEnergySupplies.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task Redirects_to_prepare_to_transfer_when_post_to_future_supply()
        {
            _futureSupplyResourceBuilder
                .WithDataTemplate()
                    .WithGroup("futureSupply")
                        .WithItem("id");
            StubFutureSupply();

            var nextResource = new ResourceBuilder()
                .WithLink("/rels/next", "next-url")
                .Build();

            _modelSaver.Save(_futureSupplyModel).Returns(new Response(nextResource));

            var viewModel = new FutureSupplyViewModel
            {
                FutureSupplyUri = _uri,
                FutureTariffId = "the tariff id"
            };

            var result = (RedirectToActionResult)await _sut.Index(viewModel);

            Assert.That(result.ActionName, Is.EqualTo("Index"));
            Assert.That(result.ControllerName, Is.EqualTo("PrepareForTransfer"));
            Assert.That(result.RouteValues["uri"], Is.EqualTo("next-url"));
        }

        [Test]
        public async Task Post_with_any_errors_returns_errors()
        {
            _futureSupplyResourceBuilder
                .WithDataTemplate()
                    .WithGroup("futureSupply")
                        .WithItem("id")
                .WithError("some-future-supply-error");

            _futureSuppliesResourceBuilder.WithResult(new Result
            {
                SupplyType = new SupplyType
                {
                    Id = "4"
                },
                EnergySupplies = new EnergySupply[] { }
            });

            StubFutureSupply();
            StubFutureSupplies();

            _modelSaver
                .Save(_futureSupplyModel)
                .Returns(new Response(_futureSupplyResource));

            var requestViewModel = new FutureSupplyViewModel { FutureSupplyUri = _uri, FutureTariffId = "the tariff id" };
            var result = (ViewResult)await _sut.Index(requestViewModel);
            var responseViewModel = (FutureSupplyViewModel)result.Model;

            var error = responseViewModel.Errors.Single();
            Assert.That(error, Is.EqualTo("some-future-supply-error"));
        }

        private void StubFutureSupply()
        {
            _futureSupplyResource = _futureSupplyResourceBuilder.Build();
            _futureSupplyModel = new FutureSupplyModel(_futureSupplyResource);
            _modelLoader.Load<Resource, FutureSupplyModel>(_uri).Returns(_futureSupplyModel);
        }

        private void StubFutureSupplies()
        {
            _modelLoader.Load<FutureSupplies, ResultsModel>(_futureSuppliesUri).Returns(new ResultsModel(_futureSuppliesResourceBuilder.Build()));

        }
    }
}
