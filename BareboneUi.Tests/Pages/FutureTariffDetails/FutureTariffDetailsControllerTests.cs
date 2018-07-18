using System;
using System.Linq;
using System.Threading.Tasks;
using BareboneUi.Common;
using BareboneUi.Pages.FutureTariffDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace BareboneUi.Tests.Pages.FutureTariffDetails
{
    [TestFixture]
    public class FutureTariffDetailsControllerTests
    {
        private FutureTariffDetailsController _futureTariffDetailsController;
        private ILoadModel _modelLoader;
        private IApiClient _apiClientStub;
        private FutureTariffDetail _futureTariffDetail;
        private string Url = "future-tariff-details";

        [SetUp]
        public void SetUp()
        {
            _futureTariffDetail = new FutureTariffDetail
            {
                Supplies = new[]
                {
                    new FutureTariffDetailsSupply()
                }
            };

            _apiClientStub = Substitute.For<IApiClient>();
            _apiClientStub.GetAsync<FutureTariffDetail>(Url).Returns(_futureTariffDetail);
            _modelLoader = new ModelRepository(_apiClientStub, Substitute.For<IConfiguration>());
            _futureTariffDetailsController = new FutureTariffDetailsController(_modelLoader);
        }

        [TestCase("electricity")]
        [TestCase("gas")]
        public async Task Maps_fuel(string fuel)
        {
            SetUpFutureTariffDetailsSupply(x => x.Fuel = fuel);

            var model = await GetFutureTariffDetailsModel();

            AssertSupplyPropertyMatches(model, x => x.Fuel, fuel);
        }

        [Test]
        public async Task Maps_supplier_description()
        {
            const string supplierDescription = "supplier description";
            SetUpFutureTariffDetailsSupply(x => x.Supplier = new FutureTariffDetailsSupplier
            {
                Description = supplierDescription
            });

            var model = await GetFutureTariffDetailsModel();

            AssertSupplyPropertyMatches(model, x => x.Supplier.Description, supplierDescription);
        }

        [Test]
        public async Task Maps_supplier_name()
        {
            const string supplierName = "supplier name";
            SetUpFutureTariffDetailsSupply(x => x.Supplier = new FutureTariffDetailsSupplier
            {
                Name = supplierName
            });

            var model = await GetFutureTariffDetailsModel();

            AssertSupplyPropertyMatches(model, x => x.Supplier.Name, supplierName);
        }

        [Test]
        public async Task Maps_CO2EmissionPerKwh()
        {
            const decimal co2EmissionPerKwh = 0.123m;
            SetUpEnvironment(x => x.CO2EmissionPerKwh = co2EmissionPerKwh);

            var model = await GetFutureTariffDetailsModel();

            AssertSupplyPropertyMatches(model, x => x.Supplier.Environmental.CO2EmissionPerKwh, co2EmissionPerKwh);
        }

        [Test]
        public async Task Maps_NuclearWastePerKwh()
        {
            const decimal nuclearWastePerKwh = 0.123m;
            SetUpEnvironment(x => x.NuclearWastePerKwh = nuclearWastePerKwh);

            var model = await GetFutureTariffDetailsModel();

            AssertSupplyPropertyMatches(model, x => x.Supplier.Environmental.NuclearWastePerKwh, nuclearWastePerKwh);
        }

        [Test]
        public async Task Maps_TariffName()
        {
            const string tariffName = "some tariff";
            SetUpFutureTariffDetailsSupply(x => x.SupplierTariff = new FutureTariffDetailsSupplierTariff
            {
                Name = tariffName
            });

            var model = await GetFutureTariffDetailsModel();

            AssertSupplyPropertyMatches(model, x => x.SupplierTariff.Name, tariffName);
        }

        [Test]
        public async Task Maps_TariffType()
        {
            const string tariffType = "some tariff type";
            SetUpFutureTariffDetailsSupply(x => x.SupplierTariff = new FutureTariffDetailsSupplierTariff
            {
                TariffType = tariffType
            });

            var model = await GetFutureTariffDetailsModel();

            AssertSupplyPropertyMatches(model, x => x.SupplierTariff.TariffType, tariffType);
        }

        [Test]
        public async Task Maps_PaymentMethod()
        {
            var paymentMethod = new KeyPair { Id = "payment method id", Name = "payment method name" };
            SetUpFutureTariffDetailsSupply(x => x.SupplierTariff = new FutureTariffDetailsSupplierTariff
            {
                PaymentMethod = paymentMethod
            });

            var model = await GetFutureTariffDetailsModel();

            AssertSupplyPropertyMatches(model, x => x.SupplierTariff.PaymentMethod.Id, paymentMethod.Id);
            AssertSupplyPropertyMatches(model, x => x.SupplierTariff.PaymentMethod.Name, paymentMethod.Name);
        }

        [Test]
        public async Task Maps_UnitCharge()
        {
            const string unitCharge = "5.000p per kWh";
            SetUpFutureTariffDetailsSupply(x => x.SupplierTariff = new FutureTariffDetailsSupplierTariff
            {
                UnitCharge = unitCharge
            });

            var model = await GetFutureTariffDetailsModel();

            AssertSupplyPropertyMatches(model, x => x.SupplierTariff.UnitCharge, unitCharge);
        }

        [Test]
        public async Task Maps_StandingCharge()
        {
            const string standingCharge = "0.00p per day (£0.00 per year)";
            SetUpFutureTariffDetailsSupply(x => x.SupplierTariff = new FutureTariffDetailsSupplierTariff
            {
                StandingCharge = standingCharge
            });

            var model = await GetFutureTariffDetailsModel();

            AssertSupplyPropertyMatches(model, x => x.SupplierTariff.StandingCharge, standingCharge);
        }

        [Test]
        public async Task Maps_TariffEndDate()
        {
            var tariffEndDate = new DateTime(2018, 5, 22, 17, 0, 0);
            SetUpFutureTariffDetailsSupply(x => x.SupplierTariff = new FutureTariffDetailsSupplierTariff
            {
                TariffEndDate = tariffEndDate
            });

            var model = await GetFutureTariffDetailsModel();

            AssertSupplyPropertyMatches(model, x => x.SupplierTariff.TariffEndDate, tariffEndDate);
        }

        [Test]
        public async Task Maps_PriceGuaranteedUntil()
        {
            var priceGuaranteedUntil = new DateTime(2018, 5, 22, 17, 0, 0);
            SetUpFutureTariffDetailsSupply(x => x.SupplierTariff = new FutureTariffDetailsSupplierTariff
            {
                PriceGuaranteedUntil = priceGuaranteedUntil
            });

            var model = await GetFutureTariffDetailsModel();

            {    AssertSupplyPropertyMatches(model, x => x.SupplierTariff.PriceGuaranteedUntil, priceGuaranteedUntil);}
        }

        [Test]
        public async Task Maps_ExitFees()
        {
            var exitFees = 1.23m;
            SetUpFutureTariffDetailsSupply(x => x.SupplierTariff = new FutureTariffDetailsSupplierTariff
            {
                ExitFees = exitFees
            });

            var model = await GetFutureTariffDetailsModel();

            {    AssertSupplyPropertyMatches(model, x => x.SupplierTariff.ExitFees, exitFees);}
        }

        [Test]
        public async Task Maps_Discounts()
        {
            var discounts = "some discount";
            SetUpFutureTariffDetailsSupply(x => x.SupplierTariff = new FutureTariffDetailsSupplierTariff
            {
                Discounts = discounts
            });

            var model = await GetFutureTariffDetailsModel();

            {    AssertSupplyPropertyMatches(model, x => x.SupplierTariff.Discounts, discounts);}
        }

        [Test]
        public async Task Maps_OtherServices()
        {
            var otherServices = "other services";
            SetUpFutureTariffDetailsSupply(x => x.SupplierTariff = new FutureTariffDetailsSupplierTariff
            {
                OtherServices = otherServices
            });

            var model = await GetFutureTariffDetailsModel();

            {    AssertSupplyPropertyMatches(model, x => x.SupplierTariff.OtherServices, otherServices);}
        }

        private void SetUpFutureTariffDetailsSupply(Action<FutureTariffDetailsSupply> configure)
        {
            configure(_futureTariffDetail.Supplies.First());
        }

        private void SetUpEnvironment(Action<FutureTariffDetailsSupplierEnvironmental> configure)
        {
            _futureTariffDetail.Supplies.First().Supplier = new FutureTariffDetailsSupplier
            {
                Environmental = new FutureTariffDetailsSupplierEnvironmental()
            };

            configure(_futureTariffDetail.Supplies.First().Supplier.Environmental);
        }

        private async Task<FutureTariffDetailsViewModel> GetFutureTariffDetailsModel()
        {
            var viewResult = (ViewResult) await _futureTariffDetailsController.Index(Url);
            return (FutureTariffDetailsViewModel) viewResult.Model;
        }

        private void AssertSupplyPropertyMatches<T>(
            FutureTariffDetailsViewModel model,
            Func<FutureTariffDetailsSupply, T> accessor,
            T expected)
        {
            Assert.That(accessor(model.Supplies.Single()), Is.EqualTo(expected));
        }
    }
}
