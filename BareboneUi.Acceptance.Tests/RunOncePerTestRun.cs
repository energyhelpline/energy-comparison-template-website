using BareboneUi.Acceptance.Tests.Infrastructure;
using NUnit.Framework;

namespace BareboneUi.Acceptance.Tests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        private LocalWebServer _webserver;
        private Chrome _chrome;

        [OneTimeSetUp]
        public void StartWebServer()
        {
            _webserver = new LocalWebServer();
            _webserver.Start();

            _chrome = new Chrome();
        }

        [OneTimeTearDown]
        public void StopWebServer()
        {
            _webserver.Stop();
            _chrome.Dispose();
        }
    }
}
