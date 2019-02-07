using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core.Configere;
using WebServer.Core.MiddleWare;
using static WebServer.Core.Configere.ApplicationBuilder;

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
