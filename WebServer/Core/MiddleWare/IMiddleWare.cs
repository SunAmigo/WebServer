namespace WebServer.Core.MiddleWare
{
    public interface IMiddleWare
    {
        void Invoke(WebContext context);
    }
}
