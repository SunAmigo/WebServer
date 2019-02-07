using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using WebServer.Core;
using WebServer.Core.Configere;
using WebServer.Core.DependencyInjection;

namespace WebServer
{
    public class WebHost
    {
        public const String VERSION = "HTTP/1.0";
        public const String SERVERNAME = "WebServer/1.0";

        private TcpListener listener;

        private Type _startupClass;
        private String _webRootPath;

        #region Property
        public Type StartupClass
        {
            get { return _startupClass; }
            set { _startupClass = value; }
        }

        public String WebRootPath
        {
            get { return _webRootPath; }
            set { _webRootPath = value; }
        }
        #endregion

        public WebHost(Int32 port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Run()
        {
            listener.Start();

            #region  
            var obj = Activator.CreateInstance(_startupClass);
            var app = ApplicationBuilder.GetInstance();
            var services = ServiceCollection.GetInstance();

            MethodInfo method = _startupClass.GetMethod("ConfigureServices");
            method.Invoke(obj, new object[] { services });

            method = _startupClass.GetMethod("Configure");
            method.Invoke(obj, new object[] { app });
            #endregion

            Logger.Log("Server is running: localhost:8888");
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
            var context = new WebContext(client);

            ApplicationBuilder.GetInstance()
                              .StartMiddleware(context);
            client.Close();
        }
    }
}
