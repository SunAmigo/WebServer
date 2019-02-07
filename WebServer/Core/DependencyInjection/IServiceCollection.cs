using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.DependencyInjection
{
    public interface IServiceCollection
    {
        void AddService<TInterface, T>();
        void AddService<T>();
    }
}
