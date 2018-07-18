using System.Linq;
using BareboneUi.Acceptance.Tests.Common;
using NUnit.Framework;

namespace BareboneUi.Acceptance.Tests.Errors
{
    [TestFixture]
    public class PrepareForTransferErrors
    {
        [Test]
        public void Prepare_for_transfer_page_displays_errors_from_api()
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
                customer.ContractExpiryDateForGas("20/08/2049");
                customer.ContractExpiryDateForElectricity("20/08/2049");
                customer.SubmitContractExpiryDate();

                customer.SelectGasUsageType("4");
                customer.SelectElectricityUsageType("4");

                customer.EnterGasSpend("123");
                customer.EnterElectricitySpend("123");
                customer.SubmitCurrentUsage();

                customer.SubmitPreferences();

                customer.SelectCheapestDualFuelTariff();

                customer.EnterCallbackUrl("test-url");

                customer.SubmitPrepareToTransfer();


                var errors = customer.GetErrors().ToList();

                Assert.That(errors, Has.Count.EqualTo(2));
                Assert.Contains("Incorrect absolute URL type", errors);
                Assert.Contains("Missing mandatory item", errors);
            }
        }
    }
}