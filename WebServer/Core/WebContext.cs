using System;
using System.Linq;
using System.Net.Sockets;
using WebServer.Core.DependencyInjection;
using WebServer.Core.MVC.Result;
using WebServer.Core.Requests;
using System.Reflection;
using System.Text;

namespace WebServer.Core
{
    public class WebContext
    {
        public enum TypeRequest
        {
            GET, POST
        };

        public enum TypeError
        {
            _200=200, _404 = 404, _405 = 405
        };


        public ShareInfo items { get; } = new ShareInfo();
        public Request Request { get; set; }
        public Response Response { get; set; }

        public Assembly Assembly { get; set; }

        public TcpClient client { get; set; }


        public WebContext(TcpClient client,Assembly asm)
        {
            this.client = client;
            Assembly = asm;
        }

        public void Initialize()
        {
            Response = new Response(client.GetStream());
            var (type,msg) = GetTypeRequest(client.GetStream());

            Request = RequestBuilder.InitializeRequest(msg, client, type);

            Logger.Log($"Request:{Environment.NewLine}{Request}");
            Console.WriteLine();

        }

        private (TypeRequest,string) GetTypeRequest(NetworkStream stream)
        {
            const char newLine = '\n';
            var msg = default(String);

            byte[] buffer = new byte[1024];
            
            
            while (stream.DataAvailable)
            {
                stream.Read(buffer, 0, buffer.Length);
                msg += Encoding.ASCII.GetString(buffer).TrimEnd();
            }
            if (msg == null)
            {
                throw new Exception("Request string is empty");
            }
            var typeString = msg.Split(newLine).FirstOrDefault().Split(' ')[0].ToString() ?? String.Empty;
            var type = default(TypeRequest);
            switch (typeString)
            {
                case "GET":  type = TypeRequest.GET; break;
                case "POST": type = TypeRequest.POST; break;
                default:
                    ViewResult.Render(TypeError._405, this);
                    throw new Exception("Method not recognized!");
            }
            return (type,msg);
        }

        public T GetService<T>()
        {
            return ServiceCollection.GetInstance().GetService<T>();
        }

    }
}
