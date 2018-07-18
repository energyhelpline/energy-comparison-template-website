using BareboneUi.Common;
using BareboneUi.Pages.CurrentSupply;
using BareboneUi.Tests.Common;
using NSubstitute;
using NUnit.Framework;

namespace BareboneUi.Tests.Pages.CurrentSupply
{
    [TestFixture]
    public class CurrentSupplyModelTests
    {
        private CurrentSupplyModel _sut;
        private Resource _resource;

        [SetUp]
        public void SetUp()
        {
            _resource = new ResourceBuilder()
                .WithDataTemplate()
                    .WithGroup("includedFuels")
                        .WithItem("compareGas")
                        .WithItem("compareElec")
                    .WithGroup("gasTariff")
                        .WithItem("supplier")
                            .WithData("default-gas-supplier")
                        .WithItem("supplierTariff")
                            .WithData("default-gas-supplier-tariff")
                        .WithItem("paymentMethod")
                            .WithData("default-gas-payment-method")
                    .WithGroup("elecTariff")
                        .WithItem("supplier")
                            .WithData("default-electricity-supplier")
                        .WithItem("supplierTariff")
                            .WithData("default-electricity-supplier-tariff")
                        .WithItem("paymentMethod")
                            .WithData("default-electricity-payment-method")
                        .WithItem("economy7")
                            .WithData("default-electricity-7")
                .Build();

            _sut = new CurrentSupplyModel(_resource);
        }

        [TestCase("includedFuels", "compareGas", "True")]
        [TestCase("includedFuels", "compareElec", "True")]
        [TestCase("gasTariff", "supplier", "gasSupplier")]
        [TestCase("gasTariff", "supplierTariff", "gasSupplierTariff")]
        [TestCase("gasTariff", "paymentMethod", "gasPaymentMethod")]
        [TestCase("elecTariff", "supplier", "electricitySupplier")]
        [TestCase("elecTariff", "supplierTariff", "electricitySupplierTariff")]
        [TestCase("elecTariff", "paymentMethod", "electricityPaymentMethod")]
        [TestCase("elecTariff", "economy7", "True")]
        public void Post_current_supply_to_the_api(string group, string item, string expected)
        {
            var answer = Substitute.For<ICurrentSupplyAnswer>();
            answer.IsGasComparison.Returns(true);
            answer.IsElectricityComparison.Returns(true);
            answer.Economy7.Returns(true);
            answer.GasSupplier.Returns("gasSupplier");
            answer.GasSupplierTariff.Returns("gasSupplierTariff");
            answer.GasSupplierPaymentMethod.Returns("gasPaymentMethod");
            answer.ElectricitySupplier.Returns("electricitySupplier");
            answer.ElectricitySupplierTariff.Returns("electricitySupplierTariff");
            answer.ElectricitySupplierPaymentMethod.Returns("electricityPaymentMethod");

            _sut.Update(answer);

            Assert.That(_resource.DataTemplate.GetItem(group, item).Data, Is.EqualTo(expected));
        }

        [TestCase("includedFuels", "compareGas", "True")]
        [TestCase("includedFuels", "compareElec", "False")]
        [TestCase("gasTariff", "supplier", "gasSupplier")]
        [TestCase("gasTariff", "supplierTariff", "gasSupplierTariff")]
        [TestCase("gasTariff", "paymentMethod", "gasPaymentMethod")]
        [TestCase("elecTariff", "supplier", "default-electricity-supplier")]
        [TestCase("elecTariff", "supplierTariff", "default-electricity-supplier-tariff")]
        [TestCase("elecTariff", "paymentMethod", "default-electricity-payment-method")]
        [TestCase("elecTariff", "economy7", "default-electricity-7")]
        public void Post_current_supply_to_the_api_as_single_gas(string group, string item, string expected)
        {
            var answer = Substitute.For<ICurrentSupplyAnswer>();
            answer.IsGasComparison.Returns(true);
            answer.IsElectricityComparison.Returns(false);
            answer.Economy7.Returns(false);
            answer.GasSupplier.Returns("gasSupplier");
            answer.GasSupplierTariff.Returns("gasSupplierTariff");
            answer.GasSupplierPaymentMethod.Returns("gasPaymentMethod");
            answer.ElectricitySupplier.Returns(string.Empty);
            answer.ElectricitySupplierTariff.Returns(string.Empty);
            answer.ElectricitySupplierPaymentMethod.Returns(string.Empty);

            _sut.Update(answer);

            Assert.That(_resource.DataTemplate.GetItem(group, item).Data, Is.EqualTo(expected));
        }

        [TestCase("includedFuels", "compareGas", "False")]
        [TestCase("includedFuels", "compareElec", "True")]
        [TestCase("gasTariff", "supplier", "default-gas-supplier")]
        [TestCase("gasTariff", "supplierTariff", "default-gas-supplier-tariff")]
        [TestCase("gasTariff", "paymentMethod", "default-gas-payment-method")]
        [TestCase("elecTariff", "supplier", "electricitySupplier")]
        [TestCase("elecTariff", "supplierTariff", "electricitySupplierTariff")]
        [TestCase("elecTariff", "paymentMethod", "electricityPaymentMethod")]
        [TestCase("elecTariff", "economy7", "True")]
        public void Post_current_supply_to_the_api_as_single_electricity(string group, string item, string expected)
        {
            var answer = Substitute.For<ICurrentSupplyAnswer>();
            answer.IsGasComparison.Returns(false);
            answer.IsElectricityComparison.Returns(true);
            answer.Economy7.Returns(true);
            answer.GasSupplier.Returns(string.Empty);
            answer.GasSupplierTariff.Returns(string.Empty);
            answer.GasSupplierPaymentMethod.Returns(string.Empty);
            answer.ElectricitySupplier.Returns("electricitySupplier");
            answer.ElectricitySupplierTariff.Returns("electricitySupplierTariff");
            answer.ElectricitySupplierPaymentMethod.Returns("electricityPaymentMethod");

            _sut.Update(answer);

            Assert.That(_resource.DataTemplate.GetItem(group, item).Data, Is.EqualTo(expected));
        }
    }
}