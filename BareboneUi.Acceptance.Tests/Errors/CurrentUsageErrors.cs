using System.Linq;
using BareboneUi.Acceptance.Tests.Common;
using NUnit.Framework;

namespace BareboneUi.Acceptance.Tests.Errors
{
    [TestFixture]
    public class CurrentUsageErrors
    {
        [Test]
        public void Current_usage_page_displays_errors_from_api()
        {
            using (var customer = new Customer())
            {
                customer.EnterPostcode("se1 0es");
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

                customer.SubmitCurrentUsage();

                var errors = customer.GetErrors().ToList();

                Assert.That(errors, Has.Count.EqualTo(2));
                Assert.Contains("Please enter your gas usage.", errors);
                Assert.Contains("Please enter your electricity usage.", errors);
            }
        }
    }
}