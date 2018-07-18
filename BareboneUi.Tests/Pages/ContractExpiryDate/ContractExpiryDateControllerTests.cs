using System;
using System.Threading.Tasks;
using BareboneUi.Common;
using BareboneUi.Pages.ContractExpiryDate;
using BareboneUi.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace BareboneUi.Tests.Pages.ContractExpiryDate
{
    [TestFixture]
    public class ContractExpiryDateControllerTests
    {
        private const string _uri = "contract-expiry-date-uri";

        private ContractExpiryDateController _sut;
        private ILoadModel _modelLoader;
        private ISaveModel _modelSaver;
        private ContractExpiryDateModel _model;
        private Resource _resource;

        [SetUp]
        public void SetUp()
        {
            _resource = new ResourceBuilder()
                .WithDataTemplate()
                    .WithGroup("gasContractDetails")
                        .WithItem("expiryDate")
                    .WithGroup("elecContractDetails")
                        .WithItem("expiryDate")
                .Build();

            _model = new ContractExpiryDateModel(_resource);

            _modelLoader = Substitute.For<ILoadModel>();
            _modelSaver = Substitute.For<ISaveModel>();
            _modelLoader.Load<Resource, ContractExpiryDateModel>(_uri).Returns(_model);
            _sut = new ContractExpiryDateController(_modelLoader, _modelSaver);
        }

        [Test]
        public void Adds_query_string_uri_to_current_usage_view_model()
        {
            var result = (ViewResult) _sut.Index(_uri);
            var viewModel = (ContractExpiryDateViewModel) result.Model;

            Assert.That(viewModel.ContractExpiryDateUri, Is.EqualTo(_uri));
        }

        [Test]
        public async Task Redirects_to_current_usage_when_post_to_contract_expiry_date_resource()
        {
            var responseStub = Substitute.For<IResponse>();
            responseStub.GetNextUrl().Returns("next-url");

            _modelSaver.Save(_model).Returns(responseStub);

            var viewModel = new ContractExpiryDateViewModel
            {
                ContractExpiryDateUri = _uri
            };

            var result = (RedirectToActionResult) await _sut.Index(viewModel);

            Assert.That(result.ActionName, Is.EqualTo("Index"));
            Assert.That(result.ControllerName, Is.EqualTo("CurrentUsage"));
            Assert.That(result.RouteValues["uri"], Is.EqualTo("next-url"));
        }

        [Test]
        public async Task Updates_the_contract_expiry_dates_in_data_template()
        {
            var gasExpiryDate = DateTime.Parse("20/10/2018");
            var electricExpiryDate = DateTime.Parse("21/11/2019");

            var viewModel = new ContractExpiryDateViewModel
            {
                ContractExpiryDateUri = _uri,
                GasContractExpiryDate = gasExpiryDate,
                ElectricityContractExpiryDate = electricExpiryDate
            };

            await _sut.Index(viewModel);

            Assert.That(_resource.DataTemplate.GetItem("gasContractDetails", "expiryDate").Data, Is.EqualTo("2018-10-20T00:00:00"));
            Assert.That(_resource.DataTemplate.GetItem("elecContractDetails", "expiryDate").Data, Is.EqualTo("2019-11-21T00:00:00"));
        }
    }
}
