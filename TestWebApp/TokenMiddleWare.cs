using WebServer.Core;
using WebServer.Core.MiddleWare;

namespace TestWebApp
{
    internal class TokenMiddleWare : MiddleWareBase
    {
        public override void Invoke(WebContext context)
        {
            context.Response.Write("Token MiddleWare");
            //_next.Invoke(context);
        }
    }
}
