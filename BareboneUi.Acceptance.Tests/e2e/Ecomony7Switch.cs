using BareboneUi.Acceptance.Tests.Common;
using NUnit.Framework;

namespace BareboneUi.Acceptance.Tests.e2e
{
    [TestFixture]
    public class Ecomony7Switch
    {
        [Test]
        public void Prepare_customer_for_transfer_when_ecomony_7_switch_completed()
        {
            using (var customer = new Customer())
            {
                customer.EnterPostcode("SE1 0ES");
                customer.SubmitPostcode();

                customer.GasComparison();
                customer.EnterGasSupplier(
                    new CurrentSupplier
                    {
                        PaymentMethod = "Pay On Receipt Of Bill",
                        Name = "British Gas",
                        Tariff = "Standard"
                    });
                customer.ElectricityComparison();
                customer.Ecomony7();
                customer.EnterElectricitySupplier(
                    new CurrentSupplier
                    {
                        PaymentMethod = "Pay On Receipt Of Bill",
                        Name = "EDF Energy",
                        Tariff = "Standard (Variable)"
                    });
                customer.SubmitCurrentSupplier();

                customer.NightPercentageUsage("42%");
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

                Assert.That(customer.GetPostCode(), Is.EqualTo("SE1 0ES"));
                Assert.That(customer.GasSupplier(), Is.EqualTo("British Gas"));
                Assert.That(customer.GasSupplierTariff(), Is.EqualTo("Standard"));
                Assert.That(customer.GasSupplierPaymentMethod(), Is.EqualTo("Pay On Receipt Of Bill"));
                Assert.That(customer.ElectricityEcomony7(), Is.EqualTo(true));
                Assert.That(customer.ElectricitySupplier(), Is.EqualTo("EDF Energy"));
                Assert.That(customer.ElectricitySupplierTariff(), Is.EqualTo("Standard (Variable)"));
                Assert.That(customer.ElectricitySupplierPaymentMethod(), Is.EqualTo("Pay On Receipt Of Bill"));
                Assert.That(customer.NightPercentUsage(), Is.EqualTo("42%"));
                Assert.That(customer.GetGasUsageForSimpleEstimator(), Is.EqualTo("Medium (house or large flat)"));
                Assert.That(customer.GetElectricityUsageForSimpleEstimator(), Is.EqualTo("Medium (house or large flat)"));
                Assert.That(customer.GetTransferUrl(), Contains.Substring("https://refresh.staging.energyhelpline.com/domestic/energy/signup/"));
            }
        }
    }
}