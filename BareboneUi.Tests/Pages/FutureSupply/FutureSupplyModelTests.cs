using BareboneUi.Common;
using BareboneUi.Pages.FutureSupply;
using BareboneUi.Tests.Common;
using NUnit.Framework;

namespace BareboneUi.Tests.Pages.FutureSupply
{
    [TestFixture]
    public class FutureSupplyModelTests
    {
        private Resource _resource;

        [SetUp]
        public void SetUp()
        {
            _resource = new ResourceBuilder()
                .WithDataTemplate()
                    .WithGroup("futureSupply")
                        .WithItem("id")
                .WithLinkedData("/rels/domestic/future-supplies", "uri-to-future-supplies")
                .Build();
        }

        [Test]
        public void Updates_the_future_supply_id()
        {
            new FutureSupplyModel(_resource).Update("the-tariff-id");

            Assert.That(_resource.DataTemplate.GetItem("futureSupply", "id").Data, Is.EqualTo("the-tariff-id"));
        }

        [Test]
        public void Gets_the_results_uri()
        {
            var futureSupply = new FutureSupplyModel(_resource);

            Assert.That(futureSupply.ResultsUri, Is.EqualTo("uri-to-future-supplies"));
        }
    }
}