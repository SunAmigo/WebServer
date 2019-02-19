using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.DependencyInjection
{
    public sealed class ServiceCollection : IServiceCollection
    {
        private static readonly ServiceCollection instance = new ServiceCollection();
        private readonly List<Service> _services = new List<Service>();

        private ServiceCollection() { }
        public static ServiceCollection GetInstance()
        {
            return instance;
        }
        public void AddService<TInterface, T>()
        {
            _services.Add(
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
            foreach (var service in _services)
            {
                var typeService = service.ServiceType;
                if (typeof(T) == typeService)
                {
                    return (T)Activator.CreateInstance(service.ImplementationType);
                }
            }
            throw new ArgumentOutOfRangeException($"service {nameof(T)} not found");
        }
    }
}
