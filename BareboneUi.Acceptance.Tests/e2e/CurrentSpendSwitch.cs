using BareboneUi.Acceptance.Tests.Common;
using NUnit.Framework;

namespace BareboneUi.Acceptance.Tests.e2e
{
    [TestFixture]
    public class CurrentSpendSwitch
    {
        [Test]
        public void Customer_can_enter_current_spend()
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

                customer.SelectGasUsageType("4");
                customer.SelectElectricityUsageType("4");

                customer.EnterGasSpend("123");
                customer.EnterElectricitySpend("456");

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
                Assert.That(string.IsNullOrEmpty(customer.GetTransferUrl()), Is.False);

                Assert.That(customer.GetGasCurrentSpend(),               Is.EqualTo("123"));
                Assert.That(customer.GetElectricityCurrentSpend(),       Is.EqualTo("456"));
            }
        }
    }
}