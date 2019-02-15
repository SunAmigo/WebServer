using System;

namespace WebServer.Core.MiddleWare
{
    public class MiddlewareNode : MiddleWareBase
    {
        public Action<WebContext, MiddleWareBase> _call;

        public override void Invoke(WebContext context)
        {
            _call?.Invoke(context, _next);
        }

        public MiddlewareNode(Action<WebContext, MiddleWareBase> action)
        {
            _call = action;
        }
    }
}
