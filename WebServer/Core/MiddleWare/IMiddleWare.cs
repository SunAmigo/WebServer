using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.MiddleWare
{
    public interface IMiddleWare
    {
        void Invoke(WebContext context);
    }
}
