using System.Linq;
using NUnit.Framework;

namespace BareboneUi.Acceptance.Tests.Errors
{
    [TestFixture]
    public class CurrentSupplyErrors
    {
        [Test]
        public void Current_supply_page_displays_errors_from_api()
        {
            using (var customer = new Customer())
            {
                customer.EnterPostcode("se1 0es");
                customer.SubmitPostcode();

                customer.SubmitCurrentSupplier();

                var errors = customer.GetErrors().ToList();

                Assert.That(errors, Has.Count.EqualTo(2));
                Assert.Contains("You must select at least one fuel to compare", errors);
                Assert.Contains("The tariff you have entered is not valid. Please check and re-submit. If you are unsure, please call us for help.", errors);
            }
        }
    }
}