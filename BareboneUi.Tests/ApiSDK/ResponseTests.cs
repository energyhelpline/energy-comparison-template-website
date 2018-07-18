using BareboneUi.Common;
using BareboneUi.Tests.Common;
using NUnit.Framework;

namespace BareboneUi.Tests.ApiSDK
{
    [TestFixture]
    public class ResponseTests
    {
        private Response _response;

        [SetUp]
        public void SetUp()
        {
            var resource = new ResourceBuilder()
                .WithLink("/rels/something /rels/next /rels/other", "next-url")
                .WithLink("/rels/something /rels/domestic/switch /rels/other", "switch-url")
                .WithLink("/rels/something /rels/domestic/switch-with-agent /rels/other", "switch-url")
                .Build();

            _response = new Response(resource);
        }

        [Test]
        public void Get_the_next_url()
        {
            Assert.That(_response.GetNextUrl(), Is.EqualTo("next-url"));
        }

        [Test]
        public void Get_the_switch_url()
        {
            Assert.That(_response.SwitchUrl, Is.EqualTo("switch-url"));
        }

        [TestCase("/rels/domestic/switch", true)]
        [TestCase("/rels/domestic/switch-with-agen", false)]
        public void Checks_the_existance_of_a_rel(string rel, bool expected)
        {
            Assert.That(_response.ContainsRel(rel), Is.EqualTo(expected));
        }
    }
}