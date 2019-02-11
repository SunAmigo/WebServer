using System.Configuration;
using System.IO;
using WebServer;

namespace TestWebApp
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            var port = int.Parse(ConfigurationManager.AppSettings["WebHostPort"]);
            var host = new WebHostBuilder()
                .Build(port, Directory.GetCurrentDirectory(), 
                    typeof(Startup));

            host.Run();
        }
    }
}
