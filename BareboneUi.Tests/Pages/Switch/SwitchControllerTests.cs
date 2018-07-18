using BareboneUi.Common;
using BareboneUi.Pages.Switch;
using BareboneUi.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BareboneUi.Tests.Pages.Switch
{
    public class SwitchControllerTests
    {
        private SwitchController _sut;
        private SwitchResourceBuilder _switchResourceBuilder;
        private IApiClient _apiClient;
        private ResourceBuilder _usageDataBuilder;

        [SetUp]
        public void SetUp()
        {
            _switchResourceBuilder = new SwitchResourceBuilder()
                .WithDefaultGasDetailedEstimateItems()
                .WithDefaultElectricityDetailedEstimateItems()
                .WithLinks(CreateDefaultLinks());

            _usageDataBuilder = new ResourceBuilder()
                .WithDataTemplate()
                    .WithGroup("gasSimpleEstimate")
                        .WithItem("simpleEstimate")
                            .WithAcceptableValue("1", "Low (small flat) gas")
                            .WithAcceptableValue("2", "Medium (house or large flat) gas")
                            .WithAcceptableValue("3", "High (large house) gas")
                    .WithGroup("gasDetailedEstimate")
                        .WithItem("houseType")
                            .WithAcceptableValue("Value 1", "Name 1")
                        .WithItem("numberOfBedrooms")
                            .WithAcceptableValue("Value 3", "Name 3")
                        .WithItem("mainCookingSource")
                            .WithAcceptableValue("Value 5", "Name 5")
                        .WithItem("cookingFrequency")
                            .WithAcceptableValue("Value 7", "Name 7")
                        .WithItem("centralHeating")
                            .WithAcceptableValue("Value 9", "Name 9")
                        .WithItem("numberOfOccupants")
                            .WithAcceptableValue("Value 11", "Name 11")
                        .WithItem("insulation")
                            .WithAcceptableValue("Value 13", "Name 13")
                        .WithItem("energyUsage")
                            .WithAcceptableValue("Value 15", "Name 15")
                    .WithGroup("elecSimpleEstimate")
                        .WithItem("simpleEstimate")
                            .WithAcceptableValue("1", "Low (small flat) elec")
                            .WithAcceptableValue("2", "Medium (house or large flat) elec")
                            .WithAcceptableValue("3", "High (large house) elec")
                    .WithGroup("elecDetailedEstimate")
                        .WithItem("houseType")
                            .WithAcceptableValue("Value 2", "Name 2")
                        .WithItem("numberOfBedrooms")
                            .WithAcceptableValue("Value 4", "Name 4")
                        .WithItem("mainCookingSource")
                            .WithAcceptableValue("Value 6", "Name 6")
                        .WithItem("cookingFrequency")
                            .WithAcceptableValue("Value 8", "Name 8")
                        .WithItem("centralHeating")
                            .WithAcceptableValue("Value 10", "Name 10")
                        .WithItem("numberOfOccupants")
                            .WithAcceptableValue("Value 12", "Name 12")
                        .WithItem("insulation")
                            .WithAcceptableValue("Value 14", "Name 14")
                        .WithItem("energyUsage")
                            .WithAcceptableValue("Value 16", "Name 16");

            _apiClient = Substitute.For<IApiClient>();
            StubSwitchResource();

            var modelLoader = new ModelRepository(_apiClient, Substitute.For<IConfiguration>());

            _sut = new SwitchController(modelLoader);
        }

        [Test]
        public async Task Maps_switch_resource_postcode_to_switch_view_model()
        {
            _switchResourceBuilder.WithPostCode("the-postcode");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.Postcode, Is.EqualTo("the-postcode"));
        }

        [TestCase("/Date(1494543600000+0100)/", "12/05/2017")]
        [TestCase("/Date(1494543600000)/", "12/05/2017")]
        [TestCase("/Date(1494598152000)/", "12/05/2017")]
        [TestCase(null, null)]
        [TestCase("", null)]
        public async Task Maps_switch_resource_electricity_contract_expiry_date_to_switch_view_model(string jsonDate, string expected)
        {
            _switchResourceBuilder.WithCurrentSupplyElectricityContractExpiryDate(jsonDate);

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityContractExpiryDate, Is.EqualTo(expected));
        }

        [TestCase(true, "True")]
        [TestCase(false, "False")]
        [TestCase(null, "False")]
        public async Task Maps_switch_resource_electricity_ecomony_7_to_switch_view_model(bool eco7, string expected)
        {
            _switchResourceBuilder.WithCurrentSupplyElectricityEconomy7(eco7);

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityEcomony7, Is.EqualTo(expected));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_payment_method_to_switch_view_model()
        {
            _switchResourceBuilder.WithCurrentSupplyElectricityPaymentMethodName("the-elec-payment-method");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityPaymentMethod, Is.EqualTo("the-elec-payment-method"));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_supplier_name_to_switch_view_model()
        {
            _switchResourceBuilder.WithCurrentSupplyElectricitySupplierName("electrcity-supplier-name");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricitySupplyName, Is.EqualTo("electrcity-supplier-name"));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_supplier_tariff_to_switch_view_model()
        {
            _switchResourceBuilder.WithCurrentSupplyElectricitySupplierTariffName("electrcity-supplier-tariff");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityTariff, Is.EqualTo("electrcity-supplier-tariff"));
        }

        [TestCase("dummyGroup", "dummyItem", "noData", "0%")]
        [TestCase("economy7", "nightUsagePercentage", "0.5", "50%")]
        public async Task Maps_night_percentage_usage_to_switch_view_model(string group, string item, string data, string expected)
        {
            _usageDataBuilder.WithGroup(group)
                .WithItem(item)
                    .WithData(data);

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.NightPercentUsage, Is.EqualTo(expected));
        }

        [TestCase("1", "Low (small flat) elec")]
        [TestCase("2", "Medium (house or large flat) elec")]
        [TestCase("3", "High (large house) elec")]
        public async Task Maps_switch_resource_electricity_usage_simple_estimator_to_switch_view_model(string simpleEstimateId, string expected)
        {
            _switchResourceBuilder.WithDefaultElectricitySimpleEstimateItems();
            _switchResourceBuilder.WithCurrentUsageElecUsageProfileUsageTypeid("2");
            _switchResourceBuilder.WithElectricityUsageItem(item => item.Name == "simpleEstimate", simpleEstimateId);

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityUsageSimpleEstimator, Is.EqualTo(expected));
        }

        [TestCase("/Date(1534719600000+0100)/", "20/08/2018")]
        [TestCase("/Date(1534719600000)/", "20/08/2018")]
        [TestCase("/Date(1534774152000)/", "20/08/2018")]
        [TestCase(null, null)]
        [TestCase("", null)]
        public async Task Maps_switch_resource_gas_contract_expiry_date_to_switch_view_model(string jsonDate, string expected)
        {
            _switchResourceBuilder.WithCurrentSupplyGasContractExpiryDate(jsonDate);

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasContractExpiryDate, Is.EqualTo(expected));
        }

        [Test]
        public async Task Maps_switch_resource_gas_payment_method_to_switch_view_model()
        {
            _switchResourceBuilder.WithCurrentSupplyGasPaymentMethodName("gas-payment-method");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasPaymentMethod, Is.EqualTo("gas-payment-method"));
        }

        [Test]
        public async Task Maps_switch_resource_gas_supplier_name_to_switch_view_model()
        {
            _switchResourceBuilder.WithCurrentSupplyGasSupplierName("gas-supplier-name");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasSupplyName, Is.EqualTo("gas-supplier-name"));
        }

        [Test]
        public async Task Maps_switch_resource_gas_supplier_tariff_to_switch_view_model()
        {
            _switchResourceBuilder.WithCurrentSupplyGasSupplierTariffName("gas-supplier-tariff");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasTariff, Is.EqualTo("gas-supplier-tariff"));
        }

        [TestCase("1", "Low (small flat) gas")]
        [TestCase("2", "Medium (house or large flat) gas")]
        [TestCase("3", "High (large house) gas")]
        public async Task Maps_switch_resource_gas_usage_simple_estimator_to_switch_view_model(string simpleEstimateId, string expected)
        {
            _switchResourceBuilder.WithDefaultGasSimpleEstimateItems();
            _switchResourceBuilder.WithCurrentUsageGasUsageProfileUsageTypeid("2");
            _switchResourceBuilder.WithGasUsageItem(item => item.Name == "simpleEstimate", simpleEstimateId);

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasUsageSimpleEstimator, Is.EqualTo(expected));
        }

        [Test]
        public async Task Maps_transfer_uri_to_switch_view_model()
        {
            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.TransferUrl, Is.EqualTo("transfer-uri"));
        }

        [Test]
        public async Task Maps_switch_resource_gas_house_type_to_switch_view_model()
        {
            _switchResourceBuilder.WithGasUsageItem(item => item.Name == "houseType", "Value 1");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasHouseType, Is.EqualTo("Name 1"));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_house_type_to_switch_view_model()
        {
            _switchResourceBuilder.WithElectricityUsageItem(item => item.Name == "houseType","Value 2");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityHouseType, Is.EqualTo("Name 2"));
        }

        [Test]
        public async Task Maps_switch_resource_gas_number_of_bedrooms_to_switch_view_model()
        {
            _switchResourceBuilder.WithGasUsageItem(item => item.Name == "numberOfBedrooms","Value 3");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasNumberOfBedrooms, Is.EqualTo("Name 3"));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_number_of_bedrooms_to_switch_view_model()
        {
            _switchResourceBuilder.WithElectricityUsageItem(item => item.Name == "numberOfBedrooms","Value 4");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityNumberOfBedrooms, Is.EqualTo("Name 4"));
        }

        [Test]
        public async Task Maps_switch_resource_gas_main_cooking_source_to_switch_view_model()
        {
            _switchResourceBuilder.WithGasUsageItem(item => item.Name == "mainCookingSource","Value 5");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasMainCookingSource, Is.EqualTo("Name 5"));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_main_cooking_source_to_switch_view_model()
        {
            _switchResourceBuilder.WithElectricityUsageItem(item => item.Name == "mainCookingSource","Value 6");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityMainCookingSource, Is.EqualTo("Name 6"));
        }

        [Test]
        public async Task Maps_switch_resource_gas_cooking_frequency_to_switch_view_model()
        {
            _switchResourceBuilder.WithGasUsageItem(item => item.Name == "cookingFrequency","Value 7");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasCookingFrequency, Is.EqualTo("Name 7"));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_cooking_frequency_to_switch_view_model()
        {
            _switchResourceBuilder.WithElectricityUsageItem(item => item.Name == "cookingFrequency","Value 8");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityCookingFrequency, Is.EqualTo("Name 8"));
        }

        [Test]
        public async Task Maps_switch_resource_gas_central_heating_to_switch_view_model()
        {
            _switchResourceBuilder.WithGasUsageItem(item => item.Name == "centralHeating","Value 9");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasCentralHeating, Is.EqualTo("Name 9"));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_central_heating_to_switch_view_model()
        {
            _switchResourceBuilder.WithElectricityUsageItem(item => item.Name == "centralHeating","Value 10");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityCentralHeating, Is.EqualTo("Name 10"));
        }

        [Test]
        public async Task Maps_switch_resource_gas_number_of_occupants_to_switch_view_model()
        {
            _switchResourceBuilder.WithGasUsageItem(item => item.Name == "numberOfOccupants","Value 11");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasNumberOfOccupants, Is.EqualTo("Name 11"));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_number_of_occupants_to_switch_view_model()
        {
            _switchResourceBuilder.WithElectricityUsageItem(item => item.Name == "numberOfOccupants","Value 12");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityNumberOfOccupants, Is.EqualTo("Name 12"));
        }

        [Test]
        public async Task Maps_switch_resource_gas_insulation_to_switch_view_model()
        {
            _switchResourceBuilder.WithGasUsageItem(item => item.Name == "insulation","Value 13");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasInsulation, Is.EqualTo("Name 13"));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_insulation_to_switch_view_model()
        {
            _switchResourceBuilder.WithElectricityUsageItem(item => item.Name == "insulation","Value 14");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityInsulation, Is.EqualTo("Name 14"));
        }

        [Test]
        public async Task Maps_switch_resource_gas_energy_usage_to_switch_view_model()
        {
            _switchResourceBuilder.WithGasUsageItem(item => item.Name == "energyUsage","Value 15");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasEnergyUsage, Is.EqualTo("Name 15"));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_energy_usage_to_switch_view_model()
        {
            _switchResourceBuilder.WithElectricityUsageItem(item => item.Name == "energyUsage","Value 16");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityEnergyUsage, Is.EqualTo("Name 16"));
        }

        [Test]
        public async Task Maps_switch_resource_gas_current_spend_to_switch_view_model()
        {
            _switchResourceBuilder.WithDefaultGasSpendItems();
            _switchResourceBuilder.WithGasUsageItem(item => item.Name == "usageAsSpend", "1234567");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasCurrentSpend, Is.EqualTo("1234567"));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_current_spend_to_switch_view_model()
        {
            _switchResourceBuilder.WithDefaultElectricitySpendItems();
            _switchResourceBuilder.WithElectricityUsageItem(item => item.Name == "usageAsSpend", "7654321");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityCurrentSpend, Is.EqualTo("7654321"));
        }

        [Test]
        public async Task Maps_switch_resource_gas_current_usage_as_kwh_to_switch_view_model()
        {
            _switchResourceBuilder.WithDefaultGasUsageAsKwhItems();
            _switchResourceBuilder.WithGasUsageItem(item => item.Name == "usageAsKWh", "gas-usage-kwh");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.GasCurrentUsageAsKwh, Is.EqualTo("gas-usage-kwh"));
        }

        [Test]
        public async Task Maps_switch_resource_electricity_current_usage_as_kwh_to_switch_view_model()
        {
            _switchResourceBuilder.WithDefaultElectricityUsageAsKwhItems();
            _switchResourceBuilder.WithElectricityUsageItem(item => item.Name == "usageAsKWh", "electricity-usage-kwh");

            var model = await GetDefaultSwitchViewModel();

            Assert.That(model.ElectricityCurrentUsageAsKwh, Is.EqualTo("electricity-usage-kwh"));
        }

        private async Task<SwitchViewModel> GetDefaultSwitchViewModel()
        {
            StubUsageResource();
            StubSwitchResource();

            var result = (ViewResult)await _sut.Index("switch-uri", "transfer-uri");
            return (SwitchViewModel)result.Model;
        }

        private void StubUsageResource() => _apiClient.GetAsync<Resource>("usage-uri").Returns(_usageDataBuilder.Build());

        private void StubSwitchResource() => _apiClient.GetAsync<SwitchResource>("switch-uri").Returns(_switchResourceBuilder.Build());

        private static List<Link> CreateDefaultLinks()
        {
            return new List<Link>
            {
                new Link
                {
                    Rel = "/rels/domestic/usage",
                    Uri = "usage-uri"
                }
            };
        }
    }
}
