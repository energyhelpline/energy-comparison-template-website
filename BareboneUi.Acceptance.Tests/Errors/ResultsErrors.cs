using BareboneUi.Acceptance.Tests.Common;
using NUnit.Framework;
using System.Linq;

namespace BareboneUi.Acceptance.Tests.Errors
{
    [TestFixture]
    public class ResultsErrors
    {
        [Test]
        public void Results_page_displays_errors_from_api()
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
                customer.ContractExpiryDateForGas("20/08/2053");
                customer.ContractExpiryDateForElectricity("20/08/2053");
                customer.SubmitContractExpiryDate();

                customer.SelectGasUsageType("4");
                customer.SelectElectricityUsageType("4");

                customer.EnterGasSpend("123");
                customer.EnterElectricitySpend("123");
                customer.SubmitCurrentUsage();

                customer.SubmitPreferences();

                customer.SelectFirstDualFuelTariff();

                var errors = customer.GetErrors().ToList();

                Assert.That(errors, Has.Count.EqualTo(1));
                Assert.Contains("You cannot apply for this tariff.", errors);
            }
        }
    }
}