using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BareboneUi.Acceptance.Tests.Infrastructure
{
    public class LocalWebServer
    {
        private readonly ExternalProgram _externalProgram;

        public LocalWebServer()
        {
            var workingDir = AppDomain.CurrentDomain.BaseDirectory;
            var csprojPath = Path.GetFullPath(Path.Combine(workingDir, @"..\..\..\..\BareboneUI"));
            _externalProgram = new ExternalProgram("dotnet", $"run --project {csprojPath}");
        }

        public void Start()
        {
            _externalProgram.Start();

            ServerStarting().Wait();
        }

        public void Stop()
        {
            _externalProgram.Dispose();
        }

        private async Task ServerStarting()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var httpClient = new HttpClient())
            {
                var serverStarted = false;
                while (!serverStarted)
                {
                    if (stopwatch.Elapsed > TimeSpan.FromSeconds(30))
                    {
                        throw new TimeoutException("Web server did not start after 30 seconds");
                    }

                    try
                    {
                        var httpResponseMessage = await httpClient.GetAsync("http://localhost:61336/Status");

                        if (!httpResponseMessage.IsSuccessStatusCode)
                        {
                            continue;
                        }

                        var result = await httpResponseMessage.Content.ReadAsStringAsync();

                        serverStarted = bool.Parse(result);
                    }
                    catch (HttpRequestException)
                    {
                        await Task.Delay(10);
                    }
                }
            }
        }
    }
}
