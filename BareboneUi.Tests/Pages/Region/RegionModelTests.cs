using System.Linq;
using BareboneUi.Pages.Region;
using BareboneUi.Tests.Common;
using NUnit.Framework;

namespace BareboneUi.Tests.Pages.Region
{
    [TestFixture]
    public class RegionModelTests
    {
        [Test]
        public void Gets_regions_from_acceptable_values()
        {
            var resource = new ResourceBuilder()
                .WithDataTemplate()
                    .WithGroup("electricityRegion")
                        .WithItem("region")
                            .WithAcceptableValue("1", "region-1")
                            .WithAcceptableValue("2", "region-2")
                        .WithItem("something-else")
                            .WithAcceptableValue("x", "xxxxxxx")
                .Build();

            var region = new RegionModel(resource);

            var regions = region.GetRegions();

            Assert.That(
                regions.Select(x => x.Id),
                Is.EquivalentTo(new[]{"1","2"}));

            Assert.That(
                regions.Select(x => x.Name),
                Is.EquivalentTo(new[]{"region-1","region-2"}));
        }
    }
}