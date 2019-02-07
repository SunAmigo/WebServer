using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using WebServer.Core.DependencyInjection;

namespace WebServer.Core
{
    public class WebContext
    {
        public ShareInfo items;

        private Request _request;
        private Response _response;
        private TcpClient _client;

        #region Properties
        public Request Request
        {
            get { return _request; }
        }
        public Response Response
        {
            get { return _response; }
        }

        #endregion

        public WebContext(TcpClient client)
        {
            this._client = client;
            var (type, msg) = GetStringReqsues(_client.GetStream());
            switch (type)
            {
                case "GET": _request = Request.GetRequest(msg); break;
                default: Logger.Error("Method not recognized!"); break;
            }
            _response = new Response(client.GetStream());

            items = new ShareInfo();

        }

        private (String type, String msg) GetStringReqsues(NetworkStream stream)
        {
            var reader = new StreamReader(stream);
            var msg = default(String);

            while (reader.Peek() != -1)
            {
                msg += reader.ReadLine() + "\n";
            }

            if (msg == null)
            {
                Logger.Error("Request string is empty");
                throw new Exception("Request string is empty");
            }

            var type = (from line in msg.Split(new char[] { '\n' })
                        where line.Contains("HTTP")
                        select line.Split(' ')[0].ToString())
                       .FirstOrDefault() ?? "";

            return (type, msg);
        }

        public T GetService<T>()
        {
            return ServiceCollection.GetInstance().GetService<T>();
        }

    }
}
