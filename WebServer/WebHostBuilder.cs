using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public sealed class WebHostBuilder
    {
        private WebHost _host;

        public WebHost Build(int port, string path, Type startupType)
        {
            _host = new WebHost(port) { WebRootPath = path, StartupClass = startupType };

            return _host;
        }
    }
}
