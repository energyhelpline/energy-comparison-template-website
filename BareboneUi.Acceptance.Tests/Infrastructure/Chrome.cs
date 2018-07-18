using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace BareboneUi.Acceptance.Tests.Infrastructure
{
    public class Chrome : IDisposable
    {
        private readonly ChromeDriverService _chromeDriverService;
        private const int _port = 5555;

        public Chrome()
        {
            _chromeDriverService = ChromeDriverService.CreateDefaultService();
            _chromeDriverService.Port = _port;
            _chromeDriverService.Start();
        }

        public void Dispose()
        {
            _chromeDriverService.Dispose();
        }

        public static RemoteWebDriver CreateDriver()
        {
            return new RemoteWebDriver(new Uri($"http://127.0.0.1:{_port}"), new ChromeOptions());
        }
    }
}