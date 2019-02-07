using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core.MiddleWare;


namespace WebServer.Core.Configere
{
    public class ApplicationBuilder
    {
        private static readonly ApplicationBuilder instance = new ApplicationBuilder();

        public delegate void RequestDelegate(WebContext contex);

        public MiddleWareBase _root;
        public MiddleWareBase _curent;
        private Dictionary<String, RequestDelegate> _maps;

        private ApplicationBuilder()
        {
            _maps = new Dictionary<String, RequestDelegate>();
        }
        public static ApplicationBuilder GetInstance()
        {
            return instance;
        }

        public void Use(Action<WebContext, MiddleWareBase> action)
        {
            if (_root == null)
            {
                _root = new MiddlewareNode(action);
                _curent = _root;
            }
            else
            {
                _curent._next = new MiddlewareNode(action);
                _curent = _curent._next;
            }
        }
        public void UseMiddleWare<T>()
        {
            Type type = typeof(T);
            if (type.IsSubclassOf(typeof(MiddleWareBase)))
            {
                if (_root == null)
                {
                    _root = (MiddleWareBase)Activator.CreateInstance(type);
                    _curent = _root;
                }
                else
                {
                    _curent._next = (MiddleWareBase)Activator.CreateInstance(type);
                    _curent = _curent._next;
                }
            }
            else
            {
                throw new Exception("Class must inherit from MiddleWareBase");
            }
        }
        public void UseMap(String map, RequestDelegate handler)
        {
            if (String.IsNullOrEmpty(map)) return;
            if (handler == null) return;

            if (_maps == null)
                _maps = new Dictionary<string, RequestDelegate>();

            _maps.Add(map, handler);

        }

        public void StartMiddleware(WebContext context)
        {
            _Map(context);
            _Use(context);
        }
        private void _Use(WebContext context)
        {
            if (context == null) throw new Exception();
            if (_root == null) return;
            _root.Invoke(context);
        }
        private Boolean _Map(WebContext context)
        {
            if (context == null) throw new Exception();
            foreach (var map in _maps)
            {
                var path = context.Request.path;
                if (map.Key.ToLower() == path.ToLower())
                {
                    map.Value.Invoke(context);
                    return true;
                }
            }
            return false;

        }

    }
}
