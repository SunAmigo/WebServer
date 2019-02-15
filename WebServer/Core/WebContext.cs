using System;
using System.Linq;
using System.Net.Sockets;
using System.IO;
using WebServer.Core.DependencyInjection;
using System.Reflection;

namespace WebServer.Core
{
    public class WebContext
    {
        public ShareInfo items { get; } = new ShareInfo();
        public Request Request { get; }
        public Response Response { get; }

        public Assembly Assembly { get; set; }

        private TcpClient _client;

        public WebContext(TcpClient client,Assembly asm)
        {
            this._client = client;
            var (type, msg) = GetStringReqsues(_client.GetStream());
            switch (type)
            {
                case "GET": Request = RequestBuilder.GetRequest(msg); break;
                default: Logger.Error("Method not recognized!"); break;
            }
            Response = new Response(client.GetStream());
            Assembly = asm;

        }

        private (String type, String msg) GetStringReqsues(NetworkStream stream)
        {
            const char newLine = '\n';

            var reader = new StreamReader(stream);
            var msg = default(String);

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
                        select line.Split(' ')[0].ToString())
                       .FirstOrDefault() ?? String.Empty;

            return (type, msg);
        }

        public T GetService<T>()
        {
            return ServiceCollection.GetInstance().GetService<T>();
        }

    }
}
