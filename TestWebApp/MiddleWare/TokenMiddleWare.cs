using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core;
using WebServer.Core.MiddleWare;


namespace TestWebApp.MiddleWare
{
    class TokenMiddleWare : MiddleWareBase
    {
        public override void Invoke(WebContext context)
        {
            context.Response.Write("Token MiddleWare");
            //_next.Invoke(context);
        }
    }
}
