using System;
using BareboneUi.Acceptance.Tests.Common;
using NUnit.Framework;

namespace BareboneUi.Acceptance.Tests.e2e
{
    [TestFixture]
    public class ContractExpiryDateSwitch
    {
        [Test]
        public void Prepare_customer_for_transfer_when_fuel_requires_contract_expiry_date()
        {
            using (var customer = new Customer())
            {
                customer.EnterPostcode("SE1 0ES");
                customer.SubmitPostcode();

                customer.GasComparison();
                customer.EnterGasSupplier(
                    new CurrentSupplier
                    {
                        PaymentMethod = "Monthly Direct Debit",
                        Name = "E.ON",
                        Tariff = "Age UK Fixed 1 year"
                    });
                customer.ElectricityComparison();
                customer.EnterElectricitySupplier(
                    new CurrentSupplier
                    {
                        PaymentMethod = "Monthly Direct Debit",
                        Name = "E.ON",
                        Tariff = "Age UK Fixed 1 year"
                    });
                customer.SubmitCurrentSupplier();
                customer.ContractExpiryDateForGas("20/08/2047");
                customer.ContractExpiryDateForElectricity("20/08/2047");
                customer.SubmitContractExpiryDate();

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

                Assert.That(string.Equals(customer.GetPostCode(),"SE1 0ES", StringComparison.OrdinalIgnoreCase));
                Assert.That(string.Equals(customer.GasSupplier(), "E.ON", StringComparison.OrdinalIgnoreCase));
                Assert.That(string.Equals(customer.GasSupplierTariff(), "Age UK Fixed 1 year", StringComparison.OrdinalIgnoreCase));
                Assert.That(string.Equals(customer.GasSupplierPaymentMethod(), "Monthly Direct Debit", StringComparison.OrdinalIgnoreCase));
                Assert.That(string.Equals(customer.ElectricitySupplier(), "E.ON", StringComparison.OrdinalIgnoreCase));
                Assert.That(string.Equals(customer.ElectricitySupplierTariff(), "Age UK Fixed 1 year", StringComparison.OrdinalIgnoreCase));
                Assert.That(string.Equals(customer.ElectricitySupplierPaymentMethod(), "Monthly Direct Debit", StringComparison.OrdinalIgnoreCase));
                Assert.That(string.Equals(customer.GetGasUsageForSimpleEstimator(), "Medium (house or large flat)", StringComparison.OrdinalIgnoreCase));
                Assert.That(string.Equals(customer.GetElectricityUsageForSimpleEstimator(), "Medium (house or large flat)", StringComparison.OrdinalIgnoreCase));
                Assert.That(customer.GetGasContractExpiryDate(), Is.EqualTo("20/08/2047"));
                Assert.That(customer.GetElectricityContractExpiryDate(), Is.EqualTo("20/08/2047"));
                Assert.That(customer.GetTransferUrl(), Contains.Substring("https://refresh.staging.energyhelpline.com/domestic/energy/signup/"));
            }
        }
    }
}