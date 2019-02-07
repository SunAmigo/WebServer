using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.DependencyInjection
{
    public class Service
    {
        public Type Servicetype;
        public Type ImplementationType;

        public Service(Type Servicetype, Type ImplementationType)
        {
            this.Servicetype = Servicetype;
            this.ImplementationType = ImplementationType;
        }

    }
}
