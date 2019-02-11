using System;

namespace WebServer.Core.DependencyInjection
{
    public class Service
    {
        public Type ServiceType;
        public Type ImplementationType;

        public Service(Type serviceType, Type implementationType)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
        }
    }
}
