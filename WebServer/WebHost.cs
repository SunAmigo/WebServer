using System;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using WebServer.Core;
using WebServer.Core.Configuration;
using WebServer.Core.DependencyInjection;

namespace WebServer
{
    public class WebHost
    {
        private readonly TcpListener _listener;

        public const string Version = "HTTP/1.0";
        public const string ServerName = "WebServer/1.0";

        public Type StartupClass { get; set; }

        public string WebRootPath { get; set; }

        public WebHost(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void Run()
        {
            _listener.Start();

            var obj = Activator.CreateInstance(StartupClass);
            var app = ApplicationBuilder.GetInstance();
            var services = ServiceCollection.GetInstance();

            var configureServicesMethod = StartupClass.GetMethod("ConfigureServices");
            if (configureServicesMethod == null) throw new ApplicationException(nameof(configureServicesMethod));
            configureServicesMethod.Invoke(obj, new object[] { services });
            
            var configureMethod = StartupClass.GetMethod("Configure");
            if (configureMethod == null) throw new ApplicationException(nameof(configureMethod));
            configureMethod.Invoke(obj, new object[] { app });

            Logger.Log("Server is running: localhost:8888");

            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Wait for connection...");

                var client = _listener.AcceptTcpClient();

                Console.WriteLine("Client connected");

                Task.Factory.StartNew(() => HandleClient(client));
            }
        }

        private static void HandleClient(TcpClient client)
        {
            var context = new WebContext(client);

            ApplicationBuilder
                .GetInstance()
                .StartMiddleware(context);

            client.Close();
        }
    }
}
