using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.DependencyInjection
{
    public class ServiceCollection : IServiceCollection
    {
        private static readonly ServiceCollection instance = new ServiceCollection();
        List<Service> services = new List<Service>();

        private ServiceCollection() { }
        public static ServiceCollection GetInstance()
        {
            return instance;
        }
        public void AddService<TInterface, T>()
        {
            services.Add(
                new Service(typeof(TInterface),
                typeof(T)
                ));
        }

        public void AddService<T>()
        {
            AddService<T, T>();
        }
        public T GetService<T>()
        {
            foreach (var service in services)
            {
                var typeService = service.Servicetype;
                if (typeof(T) == typeService)
                {
                    return (T)Activator.CreateInstance(service.ImplementationType);
                }
            }
            throw new Exception("");
        }
    }
}
