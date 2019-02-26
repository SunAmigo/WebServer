using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.Requests
{
    public class GET
    {
        public Dictionary<String, String> Querys;

        public GET(Dictionary<String, String> querys)
        {
            Querys = querys;
        }
      
    }
}
