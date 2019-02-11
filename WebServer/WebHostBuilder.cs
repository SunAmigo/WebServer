using System;

namespace WebServer
{
    public sealed class WebHostBuilder
    {
        private WebHost _host;

        public WebHost Build(int port, string path, Type startupType)
        {
            _host = new WebHost(port) {WebRootPath = path, StartupClass = startupType};

            return _host;
        }
    }
}
