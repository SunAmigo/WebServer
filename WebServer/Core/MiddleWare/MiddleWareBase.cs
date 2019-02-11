using System;

namespace WebServer.Core.MiddleWare
{
    public abstract class MiddleWareBase : IMiddleWare
    {
        public MiddleWareBase _next;

        public abstract void Invoke(WebContext context);
    }
}
