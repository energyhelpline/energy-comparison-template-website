using NUnit.Framework;
using System.Linq;

namespace BareboneUi.Acceptance.Tests.Errors
{
    [TestFixture]
    public class StartSwitchErrors
    {
        [Test]
        public void Page_displays_error_for_missing_postcode()
        {
            using (var customer = new Customer())
            {
                customer.SubmitPostcode();

                var errors = customer.GetErrors().ToList();

                Assert.That(errors, Has.Count.EqualTo(1));
                Assert.Contains("Missing mandatory item", errors);
            }
        }

        [Test]
        public void Page_displays_error_for_invalid_postcode()
        {
            using (var customer = new Customer())
            {
                customer.EnterPostcode("SE");

                customer.SubmitPostcode();

                var errors = customer.GetErrors().ToList();

                Assert.That(errors, Has.Count.EqualTo(1));
                Assert.Contains("Please supply a valid UK postcode", errors);
            }
        }
    }
}