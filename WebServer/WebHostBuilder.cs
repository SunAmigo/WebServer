using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class WebHostBuilder
    {
        private WebHost host;

        public WebHostBuilder()
        {
            host = new WebHost(8888);
        }

        public WebHostBuilder UseStartup<T>()
        {
            host.StartupClass = typeof(T);
            return this;
        }

        public WebHostBuilder UseWebRoot(String path)
        {
            host.WebRootPath = path;
            return this;
        }

        public WebHost Build()
        {
            return host;
        }
    }
}
