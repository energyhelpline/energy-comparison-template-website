using BareboneUi.Common;
using BareboneUi.Pages.CurrentUsage;
using BareboneUi.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BareboneUi.Tests.Pages.CurrentUsage
{
    [TestFixture]
    public class CurrentUsageControllerTests
    {
        private const string _uri = "current-usage-uri";
        private CurrentUsageController _sut;
        private Resource _resource;
        private ILoadModel _modelLoader;
        private ISaveModel _modelSaver;
        private CurrentUsageViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _resource = new ResourceBuilder()
                .WithDataTemplate()
                    .WithGroup("gasUsageType")
                        .WithItem("usageType")
                    .WithGroup("gasSimpleEstimate")
                        .WithItem("simpleEstimate")
                            .WithData("default-gas-simple-estimate")
                    .WithGroup("elecUsageType")
                        .WithItem("usageType")
                    .WithGroup("elecSimpleEstimate")
                        .WithItem("simpleEstimate")
                            .WithData("default-electricity-simple-estimate")
                    .WithGroup("gasDetailedEstimate")
                        .WithItem("houseType")
                        .WithItem("numberOfBedrooms")
                        .WithItem("mainCookingSource")
                        .WithItem("cookingFrequency")
                        .WithItem("centralHeating")
                        .WithItem("numberOfOccupants")
                        .WithItem("insulation")
                        .WithItem("energyUsage")
                    .WithGroup("elecDetailedEstimate")
                        .WithItem("houseType")
                        .WithItem("numberOfBedrooms")
                        .WithItem("mainCookingSource")
                        .WithItem("cookingFrequency")
                        .WithItem("centralHeating")
                        .WithItem("numberOfOccupants")
                        .WithItem("insulation")
                        .WithItem("energyUsage")
                    .WithGroup("gasSpend")
                        .WithItem("usageAsSpend")
                    .WithGroup("elecSpend")
                        .WithItem("usageAsSpend")
                    .WithGroup("gasKWhUsage")
                        .WithItem("usageAsKWh")
                    .WithGroup("elecKWhUsage")
                        .WithItem("usageAsKWh")
                    .WithGroup("economy7")
                        .WithItem("nightUsagePercentage")
                    .WithGroup("includedFuels")
                        .WithItem("compareGas")
                            .WithData("False")
                        .WithItem("compareElec")
                            .WithData("False")
                .Build();

            var response = Substitute.For<IResponse>();
            response.GetNextUrl().Returns("next-url");

            _viewModel = new CurrentUsageViewModel
            {
                CurrentUsageUri = _uri,
                ElectricityUsageSimpleEstimator = "electrcicitySimpleEstimateValue",
                GasUsageSimpleEstimator = "gasSimpleEstimateValue",
            };

            _modelLoader = Substitute.For<ILoadModel>();
            _modelSaver = Substitute.For<ISaveModel>();

            var model = new CurrentUsageModel(_resource);
            _modelLoader.Load<Resource, CurrentUsageModel>(_viewModel.CurrentUsageUri).Returns(model);
            _modelSaver.Save(model).Returns(response);

            _sut = new CurrentUsageController(_modelLoader, _modelSaver);
        }

        [Test]
        public async Task Adds_query_string_uri_to_current_usage_view_model()
        {
            var model = await GetDefaultCurrentUsageViewModel();

            Assert.That(model.CurrentUsageUri, Is.EqualTo(_uri));
        }

        [Test]
        public async Task Populates_current_usage_view_model_gas_simple_estimators_with_gas_simple_estimators_acceptable_values()
        {
            var values = SetupAcceptableValuesForQuestion("gasSimpleEstimate", "simpleEstimate");

            var model = await GetDefaultCurrentUsageViewModel();

            Assert.That(model.GasUsageSimpleEstimators, Is.EquivalentTo(values));
        }

        [Test]
        public async Task Populates_current_usage_view_model_electricity_simple_estimators_with_electricity_simple_estimators_acceptable_values()
        {
            var values = SetupAcceptableValuesForQuestion("elecSimpleEstimate", "simpleEstimate");

            var model = await GetDefaultCurrentUsageViewModel();

            Assert.That(model.ElectricityUsageSimpleEstimators, Is.EquivalentTo(values));
        }

        [Test]
        public async Task Populates_current_usage_view_model_house_type_with_acceptable_values()
        {
            var values = SetupAcceptableValuesForQuestion("gasDetailedEstimate", "houseType");

            var model = await GetDefaultCurrentUsageViewModel();

            Assert.That(model.HouseTypeValues, Is.EquivalentTo(values));
        }

        [Test]
        public async Task Populates_current_usage_view_model_number_of_bedrooms_with_acceptable_values()
        {
            var values = SetupAcceptableValuesForQuestion("gasDetailedEstimate", "numberOfBedrooms");

            var model = await GetDefaultCurrentUsageViewModel();

            Assert.That(model.NumberOfBedroomsValues, Is.EquivalentTo(values));
        }

        [Test]
        public async Task Populates_current_usage_view_model_main_cooking_source_with_acceptable_values()
        {
            var values = SetupAcceptableValuesForQuestion("gasDetailedEstimate", "mainCookingSource");

            var model = await GetDefaultCurrentUsageViewModel();

            Assert.That(model.MainCookingSourceValues, Is.EquivalentTo(values));
        }

        [Test]
        public async Task Populates_current_usage_view_model_cooking_frequency_with_acceptable_values()
        {
            var values = SetupAcceptableValuesForQuestion("gasDetailedEstimate", "cookingFrequency");

            var model = await GetDefaultCurrentUsageViewModel();

            Assert.That(model.CookingFrequencyValues, Is.EquivalentTo(values));
        }

        [Test]
        public async Task Populates_current_usage_view_model_central_heating_with_acceptable_values()
        {
            var values = SetupAcceptableValuesForQuestion("gasDetailedEstimate", "centralHeating");

            var model = await GetDefaultCurrentUsageViewModel();

            Assert.That(model.CentralHeatingValues, Is.EquivalentTo(values));
        }

        [Test]
        public async Task Populates_current_usage_view_model_number_of_occupants_with_acceptable_values()
        {
            var values = SetupAcceptableValuesForQuestion("gasDetailedEstimate", "numberOfOccupants");

            var model = await GetDefaultCurrentUsageViewModel();

            Assert.That(model.NumberOfOccupantsValues, Is.EquivalentTo(values));
        }

        [Test]
        public async Task Populates_current_usage_view_model_insulation_with_acceptable_values()
        {
            var values = SetupAcceptableValuesForQuestion("gasDetailedEstimate", "insulation");

            var model = await GetDefaultCurrentUsageViewModel();

            Assert.That(model.InsulationValues, Is.EquivalentTo(values));
        }

        [Test]
        public async Task Populates_current_usage_view_model_energy_usage_with_acceptable_values()
        {
            var values = SetupAcceptableValuesForQuestion("gasDetailedEstimate", "energyUsage");

            var model = await GetDefaultCurrentUsageViewModel();

            Assert.That(model.EnergyUsageValues, Is.EquivalentTo(values));
        }

        [Test]
        public async Task Redirects_to_preferences_when_post_to_current_usage_resource()
        {
            var result = (RedirectToActionResult) await _sut.Index(_viewModel);

            Assert.That(result.ActionName, Is.EqualTo("Index"));
            Assert.That(result.ControllerName, Is.EqualTo("Preferences"));
            Assert.That(result.RouteValues["uri"], Is.EqualTo("next-url"));
        }

        [TestCase("True", "False", "2", "gasSimpleEstimateValue", null, "default-electricity-simple-estimate")]
        [TestCase("True", "True", "2", "gasSimpleEstimateValue", "2", "electrcicitySimpleEstimateValue")]
        [TestCase("False", "True", null, "default-gas-simple-estimate", "2", "electrcicitySimpleEstimateValue")]
        public async Task Updates_current_usage_resource_on_post_to_current_usage_with_simple_estimator(string compareGas, string compareElec, string gasUsageType, string gasSimpleEstimate, string elecUsageType, string elecSimpleEstimate)
        {
            SetIncludedFuels(compareGas, compareElec);
            SetUsageTypes(gasUsageType, elecUsageType);

            await _sut.Index(_viewModel);

            Assert.That(_resource.DataTemplate.GetItem("includedFuels", "compareGas").Data, Is.EqualTo(compareGas));
            Assert.That(_resource.DataTemplate.GetItem("includedFuels", "compareElec").Data, Is.EqualTo(compareElec));
            Assert.That(_resource.DataTemplate.GetItem("gasUsageType", "usageType").Data, Is.EqualTo(gasUsageType));
            Assert.That(_resource.DataTemplate.GetItem("gasSimpleEstimate", "simpleEstimate").Data, Is.EqualTo(gasSimpleEstimate));
            Assert.That(_resource.DataTemplate.GetItem("elecUsageType", "usageType").Data, Is.EqualTo(elecUsageType));
            Assert.That(_resource.DataTemplate.GetItem("elecSimpleEstimate", "simpleEstimate").Data, Is.EqualTo(elecSimpleEstimate));
        }

        [TestCase("True", "False", null, null)]
        [TestCase("True", "True", "40", "0.4")]
        [TestCase("False", "True", "50", "0.5")]
        public async Task Updates_current_usage_resource_on_post_to_current_usage_with_simple_estimator(string compareGas, string compareElec, string nightUsagePercentage, string expectedNightUsagePercentage)
        {
            SetIncludedFuels(compareGas, compareElec);
            _viewModel.NightPercentageUsage = nightUsagePercentage;

            await _sut.Index(_viewModel);

            Assert.That(_resource.DataTemplate.GetItem("economy7", "nightUsagePercentage").Data, Is.EqualTo(expectedNightUsagePercentage));
        }

        [TestCase("True", "True", "1", "1", "1", "2", "3", "4", "5", "6", "7", "8", "1", "2", "3", "4", "5", "6", "7", "8")]
        [TestCase("False", "True", null, "1", null, null, null, null, null, null, null, null, "9", "10", "11", "12", "13", "14", "15", "16")]
        [TestCase("True", "False", "1", null, "1", "2", "3", "4", "5", "6", "7", "8", null, null, null, null, null, null, null, null)]
        public async Task Updates_current_usage_resource_on_post_to_current_usage_with_detailed_estimator(
            string compareGas,
            string compareElec,
            string gasUsageType,
            string elecUsageType,
            string gasHouseType,
            string gasNumberOfBedrooms,
            string gasMainCookingSource,
            string gasCookingFrequency,
            string gasCentralHeating,
            string gasNumberOfOccupants,
            string gasInsulation,
            string gasEnergyUsage,
            string electricityHouseType,
            string electricityNumberOfBedrooms,
            string electricityMainCookingSource,
            string electricityCookingFrequency,
            string electricityCentralHeating,
            string electricityNumberOfOccupants,
            string electricityInsulation,
            string electricityEnergyUsage)
        {

            SetIncludedFuels(compareGas, compareElec);
            SetUsageTypes(gasUsageType, elecUsageType);

            _viewModel.HouseType = gasHouseType ?? electricityHouseType;
            _viewModel.NumberOfBedrooms = gasNumberOfBedrooms ?? electricityNumberOfBedrooms;
            _viewModel.MainCookingSource = gasMainCookingSource ?? electricityMainCookingSource;
            _viewModel.CookingFrequency = gasCookingFrequency ?? electricityCookingFrequency;
            _viewModel.CentralHeating = gasCentralHeating ?? electricityCentralHeating;
            _viewModel.NumberOfOccupants = gasNumberOfOccupants ?? electricityNumberOfOccupants;
            _viewModel.Insulation = gasInsulation ?? electricityInsulation;
            _viewModel.EnergyUsage = gasEnergyUsage ?? electricityEnergyUsage;

            await _sut.Index(_viewModel);

            Assert.That(_resource.DataTemplate.GetItem("includedFuels", "compareGas").Data, Is.EqualTo(compareGas));
            Assert.That(_resource.DataTemplate.GetItem("includedFuels", "compareElec").Data, Is.EqualTo(compareElec));
            Assert.That(_resource.DataTemplate.GetItem("gasUsageType", "usageType").Data, Is.EqualTo(gasUsageType));
            Assert.That(_resource.DataTemplate.GetItem("elecUsageType", "usageType").Data, Is.EqualTo(elecUsageType));
            Assert.That(_resource.DataTemplate.GetItem("gasDetailedEstimate", "houseType").Data, Is.EqualTo(gasHouseType));
            Assert.That(_resource.DataTemplate.GetItem("gasDetailedEstimate", "numberOfBedrooms").Data, Is.EqualTo(gasNumberOfBedrooms));
            Assert.That(_resource.DataTemplate.GetItem("gasDetailedEstimate", "mainCookingSource").Data, Is.EqualTo(gasMainCookingSource));
            Assert.That(_resource.DataTemplate.GetItem("gasDetailedEstimate", "cookingFrequency").Data, Is.EqualTo(gasCookingFrequency));
            Assert.That(_resource.DataTemplate.GetItem("gasDetailedEstimate", "centralHeating").Data, Is.EqualTo(gasCentralHeating));
            Assert.That(_resource.DataTemplate.GetItem("gasDetailedEstimate", "numberOfOccupants").Data, Is.EqualTo(gasNumberOfOccupants));
            Assert.That(_resource.DataTemplate.GetItem("gasDetailedEstimate", "insulation").Data, Is.EqualTo(gasInsulation));
            Assert.That(_resource.DataTemplate.GetItem("gasDetailedEstimate", "energyUsage").Data, Is.EqualTo(gasEnergyUsage));
            Assert.That(_resource.DataTemplate.GetItem("elecDetailedEstimate", "houseType").Data, Is.EqualTo(electricityHouseType));
            Assert.That(_resource.DataTemplate.GetItem("elecDetailedEstimate", "numberOfBedrooms").Data, Is.EqualTo(electricityNumberOfBedrooms));
            Assert.That(_resource.DataTemplate.GetItem("elecDetailedEstimate", "mainCookingSource").Data, Is.EqualTo(electricityMainCookingSource));
            Assert.That(_resource.DataTemplate.GetItem("elecDetailedEstimate", "cookingFrequency").Data, Is.EqualTo(electricityCookingFrequency));
            Assert.That(_resource.DataTemplate.GetItem("elecDetailedEstimate", "centralHeating").Data, Is.EqualTo(electricityCentralHeating));
            Assert.That(_resource.DataTemplate.GetItem("elecDetailedEstimate", "numberOfOccupants").Data, Is.EqualTo(electricityNumberOfOccupants));
            Assert.That(_resource.DataTemplate.GetItem("elecDetailedEstimate", "insulation").Data, Is.EqualTo(electricityInsulation));
            Assert.That(_resource.DataTemplate.GetItem("elecDetailedEstimate", "energyUsage").Data, Is.EqualTo(electricityEnergyUsage));
        }

        [TestCase("True", "True", "4", "gasCurrentSpendValue", "4", "electricityCurrentSpendValue")]
        [TestCase("True", "False", "4", "gasCurrentSpendValue", null, null)]
        [TestCase("False", "True", null, null, "4", "electricityCurrentSpendValue")]
        public async Task Updates_current_usage_resource_on_post_to_current_usage_with_spend_at_default_frequency(string compareGas, string compareElec, string gasUsageType, string gasCurrentSpend, string elecUsageType, string elecCurrentSpend)
        {
            SetIncludedFuels(compareGas, compareElec);
            SetUsageTypes(gasUsageType, elecUsageType);

            _viewModel.GasCurrentSpend = gasCurrentSpend;
            _viewModel.ElectricityCurrentSpend = elecCurrentSpend;

            await _sut.Index(_viewModel);

            Assert.That(_resource.DataTemplate.GetItem("gasSpend", "usageAsSpend").Data, Is.EqualTo(gasCurrentSpend));
            Assert.That(_resource.DataTemplate.GetItem("elecSpend", "usageAsSpend").Data, Is.EqualTo(elecCurrentSpend));
            Assert.That(_resource.DataTemplate.GetItem("gasUsageType", "usageType").Data, Is.EqualTo(gasUsageType));
            Assert.That(_resource.DataTemplate.GetItem("elecUsageType", "usageType").Data, Is.EqualTo(elecUsageType));
        }

        [TestCase("True", "True", "3", "gasCurrentUsageValue", "3", "electricityCurrentUsageValue")]
        [TestCase("True", "False", "3", "gasCurrentUsageValue", null, null)]
        [TestCase("False", "True", null, null, "3", "electricityCurrentUsageValue")]
        public async Task Updates_current_usage_resource_on_post_to_current_usage_with_usage_as_kWh_at_default_frequency(string compareGas, string compareElec, string gasUsageType, string gasCurrentUsage, string elecUsageType, string elecCurrentUsage)
        {

            SetIncludedFuels(compareGas, compareElec);
            SetUsageTypes(gasUsageType, elecUsageType);

            _viewModel.GasCurrentUsageAsKwh = gasCurrentUsage;
            _viewModel.ElectricityCurrentUsageAsKwh = elecCurrentUsage;

            await _sut.Index(_viewModel);

            Assert.That(_resource.DataTemplate.GetItem("gasKWhUsage", "usageAsKWh").Data, Is.EqualTo(gasCurrentUsage));
            Assert.That(_resource.DataTemplate.GetItem("elecKWhUsage", "usageAsKWh").Data, Is.EqualTo(elecCurrentUsage));
            Assert.That(_resource.DataTemplate.GetItem("gasUsageType", "usageType").Data, Is.EqualTo(gasUsageType));
            Assert.That(_resource.DataTemplate.GetItem("elecUsageType", "usageType").Data, Is.EqualTo(elecUsageType));
        }

        [Test]
        public async Task Post_with_any_errors_returns_errors()
        {
            _modelSaver.Save(Arg.Any<CurrentUsageModel>()).Returns(new Response(new ResourceBuilder().WithError("some-error-message").Build()));

            var result = (ViewResult)await _sut.Index(_viewModel);
            var responseViewModel = (CurrentUsageViewModel)result.Model;

            var error = responseViewModel.Errors.Single();
            Assert.That(error, Is.EqualTo("some-error-message"));
        }

        private async Task<CurrentUsageViewModel> GetDefaultCurrentUsageViewModel()
        {
            var result = (ViewResult) await _sut.Index(_uri);
            return (CurrentUsageViewModel) result.Model;
        }

        private IEnumerable<KeyPair> SetupAcceptableValuesForQuestion(string groupName, string itemName)
        {
            var values = new List<KeyPair>
            {
                new KeyPair {Id = "1", Name = "value1"},
                new KeyPair {Id = "2", Name = "value2"}
            };
            _resource.DataTemplate.GetItem(groupName, itemName).AcceptableValues = values;

            return values;
        }

        private void SetIncludedFuels(string compareGas, string compareElec)
        {
            _resource.DataTemplate.GetItem("includedFuels", "compareGas").Data = compareGas;
            _resource.DataTemplate.GetItem("includedFuels", "compareElec").Data = compareElec;
        }

        private void SetUsageTypes(string gasUsageType, string elecUsageType)
        {
            _viewModel.GasUsageType = gasUsageType;
            _viewModel.ElectricityUsageType = elecUsageType;
        }
    }
}
