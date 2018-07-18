using BareboneUi.Acceptance.Tests.Common;
using NUnit.Framework;

namespace BareboneUi.Acceptance.Tests.e2e
{
    [TestFixture]
    public class CurrentUsageAsKwhSwitch
    {
        [Test]
        public void Customer_can_enter_usage_as_kWh()
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
                customer.EnterElectricitySupplier(
                    new CurrentSupplier
                    {
                        PaymentMethod = "Pay On Receipt Of Bill",
                        Name = "EDF Energy",
                        Tariff = "Standard (Variable)"
                    });
                customer.SubmitCurrentSupplier();

                customer.SelectGasUsageType("3");
                customer.SelectElectricityUsageType("3");

                customer.EnterGasUsageAsKwh("321");
                customer.EnterElectricityUsageAsKwh("654");

                customer.SubmitCurrentUsage();

                customer.EnterTariffFilterOption("All");
                customer.EnterResultsOrder("price");
                customer.EnterPaymentMethod("Monthly Direct Debit");
                customer.SubmitPreferences();

                customer.SelectCheapestDualFuelTariff();

                customer.EnterThankUrl("http://thank.com");
                customer.EnterCallbackUrl("http://callback.com");
                customer.SubmitPrepareToTransfer();

                Assert.That(customer.GetPostCode(),                      Is.EqualTo("SE1 0ES"));
                Assert.That(customer.GasSupplier(),                      Is.EqualTo("British Gas"));
                Assert.That(customer.GasSupplierTariff(),                Is.EqualTo("Standard"));
                Assert.That(customer.GasSupplierPaymentMethod(),         Is.EqualTo("Pay On Receipt Of Bill"));
                Assert.That(customer.ElectricitySupplier(),              Is.EqualTo("EDF Energy"));
                Assert.That(customer.ElectricitySupplierTariff(),        Is.EqualTo("Standard (Variable)"));
                Assert.That(customer.ElectricitySupplierPaymentMethod(), Is.EqualTo("Pay On Receipt Of Bill"));
                Assert.That(customer.GetTransferUrl(),                   Contains.Substring("https://refresh.staging.energyhelpline.com/domestic/energy/signup/"));

                Assert.That(customer.GetGasCurrentUsageAsKwh(),          Is.EqualTo("321"));
                Assert.That(customer.GetElectricityCurrentUsageAsKwh(),  Is.EqualTo("654"));
            }
        }
    }
}