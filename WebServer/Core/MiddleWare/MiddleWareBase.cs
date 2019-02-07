using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.MiddleWare
{
    public abstract class MiddleWareBase : IMiddleWare
    {
        public MiddleWareBase _next;

        public virtual void Invoke(WebContext context)
        {
            throw new NotImplementedException();
        }
    }
}
