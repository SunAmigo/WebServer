namespace WebServer.Core.DependencyInjection
{
    public interface IServiceCollection
    {
        void AddService<TInterface, T>();
        void AddService<T>();
    }
}
