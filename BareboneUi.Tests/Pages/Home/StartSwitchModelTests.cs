using BareboneUi.Common;
using BareboneUi.Pages.Home;
using BareboneUi.Tests.Common;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace BareboneUi.Tests.Pages.Home
{
    [TestFixture]
    public class StartSwitchModelTests
    {
        [TestCase("supplyPostcode", "postcode", "post-code")]
        [TestCase("references", "partnerReference", "test-api-refence")]
        [TestCase("references", "apiKey", "test-api-key")]
        public void Posts_postcode_to_the_api(string groupName, string itemName, string expected)
        {
            var resource =
                new ResourceBuilder()
                    .WithDataTemplate()
                        .WithGroup("supplyPostcode")
                            .WithItem("postcode")
                        .WithGroup("references")
                            .WithItem("partnerReference")
                            .WithItem("apiKey")
                    .Build();

            var configuration = Substitute.For<IConfiguration>();
            configuration.GetValue<string>("ApiKey").Returns("test-api-key");
            configuration.GetValue<string>("ApiPartnerReference").Returns("test-api-refence");

            var startSwitch = new StartSwitchModel(resource, configuration);
            startSwitch.Update("post-code");

            Assert.That(resource.DataTemplate.GetItem(groupName, itemName).Data, Is.EqualTo(expected));
        }
    }
}