using System;
using System.Linq;
using System.Net.Sockets;
using System.IO;
using WebServer.Core.DependencyInjection;

namespace WebServer.Core
{
    public class WebContext
    {
        public ShareInfo items { get; } = new ShareInfo();

        public Request Request { get; }

        public Response Response { get; }

        public WebContext(TcpClient client)
        {
            var (type, msg) = GetStringRequests(client.GetStream());

            switch (type)
            {
                case "GET":
                    Request = Request.GetRequest(msg);
                    break;
                default:
                    Logger.Error("Method not recognized!");
                    break;
            }

            Response = new Response(client.GetStream());
        }

        private static (string type, string msg) GetStringRequests(Stream stream)
        {
            const char newLine = '\n';

            var reader = new StreamReader(stream);
            var msg = default(string);

            while (reader.Peek() != -1)
            {
                msg += reader.ReadLine() + newLine;
            }

            if (msg == null)
            {
                Logger.Error("Request string is empty");
                throw new Exception("Request string is empty");
            }

            var type = (from line in msg.Split(newLine)
                        where line.Contains("HTTP")
                        select line.Split(' ')[0]).FirstOrDefault() ?? string.Empty;

            return (type, msg);
        }

        public T GetService<T>()
        {
            return ServiceCollection.GetInstance().GetService<T>();
        }
    }
}
