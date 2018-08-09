using BareboneUi.Acceptance.Tests.Common;
using NUnit.Framework;

namespace BareboneUi.Acceptance.Tests.e2e
{
    [TestFixture]
    public class UnknownPostcodeSwitch
    {
        [Test]
        public void Customer_enters_a_region_when_the_region_is_unknown_for_the_given_postcode()
        {
            using (var customer = new Customer())
            {
                customer.EnterPostcode("AA9 9AA");
                customer.SubmitPostcode();

                customer.EnterRegion("London");
                customer.SubmitRegion();

                customer.GasComparison();
                customer.EnterGasSupplier(
                    new CurrentSupplier
                    {
                        PaymentMethod = "Pay On Receipt Of Bill",
                        Name = "British Gas",
                        Tariff = "Standard"
                    });
                customer.ElectricityComparison();
                customer.EnterElectricitySupplier(
                    new CurrentSupplier
                    {
                        PaymentMethod = "Pay On Receipt Of Bill",
                        Name = "EDF Energy",
                        Tariff = "Standard (Variable)"
                    });
                customer.SubmitCurrentSupplier();

                customer.EnterGasUsageUsingSimpleEstimator("Medium (house or large flat)");
                customer.EnterElectricityUsageUsingSimpleEstimator("Medium (house or large flat)");
                customer.SubmitCurrentUsage();

                customer.EnterTariffFilterOption("All");
                customer.EnterResultsOrder("price");
                customer.EnterPaymentMethod("Monthly Direct Debit");
                customer.SubmitPreferences();

                customer.SelectCheapestDualFuelTariff();

                customer.EnterThankUrl("http://thank.com");
                customer.EnterCallbackUrl("http://callback.com");
                customer.SubmitPrepareToTransfer();

                Assert.That(customer.GetPostCode(), Is.EqualTo("AA9 9AA"));
                Assert.That(customer.GasSupplier(), Is.EqualTo("British Gas"));
                Assert.That(customer.GasSupplierTariff(), Is.EqualTo("Standard"));
                Assert.That(customer.GasSupplierPaymentMethod(), Is.EqualTo("Pay On Receipt Of Bill"));
                Assert.That(customer.ElectricitySupplier(), Is.EqualTo("EDF Energy"));
                Assert.That(customer.ElectricitySupplierTariff(), Is.EqualTo("Standard (Variable)"));
                Assert.That(customer.ElectricitySupplierPaymentMethod(), Is.EqualTo("Pay On Receipt Of Bill"));
                Assert.That(customer.GetGasUsageForSimpleEstimator(), Is.EqualTo("Medium (house or large flat)"));
                Assert.That(customer.GetElectricityUsageForSimpleEstimator(), Is.EqualTo("Medium (house or large flat)"));
                Assert.That(string.IsNullOrEmpty(customer.GetTransferUrl()), Is.False);
            }
        }
    }
}