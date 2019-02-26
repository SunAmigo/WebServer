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
        private readonly TcpListener listener;

        public const String Version = "HTTP/1.0";
        public const String ServerName = "WebServer/1.0";

        public Type StartupClass { get; set; }   
        public string WebRootPath { get; set; }

        private Int32 _port;

        public WebHost(Int32 port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            _port = port;
        }

        public void Run()
        {
            listener.Start();

            var obj = Activator.CreateInstance(StartupClass);
            var app = ApplicationBuilder.GetInstance();
            var services = ServiceCollection.GetInstance();

            var configureServicesMethod = StartupClass.GetMethod("ConfigureServices");
            if (configureServicesMethod == null) throw new ApplicationException(nameof(configureServicesMethod));
            configureServicesMethod.Invoke(obj, new object[] { services });

            var configureMethod = StartupClass.GetMethod("Configure");
            if (configureMethod == null) throw new ApplicationException(nameof(configureMethod));
            configureMethod.Invoke(obj, new object[] { app });

            Logger.Log($"Server is running: localhost:{_port}");

            Console.WriteLine();
            while (true)
            {
                Console.WriteLine("Wait for connection...");

                var client = listener.AcceptTcpClient();

                Console.WriteLine("Client conected");

                Task.Factory.StartNew(() => HandleClient(client));
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                var asm = StartupClass.Assembly;
                var context = new WebContext(client, asm);
                context.Initialize();

                ApplicationBuilder
                    .GetInstance()
                    .StartMiddleware(context);
            }
            catch (Exception ex)
            {
                Logger.Error($"{ex.Message}");
            }
            finally
            {
                client.Close();

            }
        }
    }
}
