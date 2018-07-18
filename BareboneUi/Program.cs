using System.IO;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace BareboneUi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var location = Path.GetFullPath(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "..\\..\\.."));
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseContentRoot(location)
                .Build();
        }
    }
}
