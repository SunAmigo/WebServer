using System;
using System.Collections.Generic;
using WebServer.Core.MiddleWare;

namespace WebServer.Core.Configuration
{
    public class ApplicationBuilder
    {
        private static readonly ApplicationBuilder Instance = new ApplicationBuilder();

        public delegate void RequestDelegate(WebContext context);

        public MiddleWareBase _root;
        public MiddleWareBase _current;

        private Dictionary<string, RequestDelegate> _maps;

        private ApplicationBuilder()
        {
            _maps = new Dictionary<string, RequestDelegate>();
        }

        public static ApplicationBuilder GetInstance()
        {
            return Instance;
        }

        public void Use(Action<WebContext, MiddleWareBase> action)
        {
            if (_root == null)
            {
                _root = new MiddlewareNode(action);
                _current = _root;
            }
            else
            {
                _current._next = new MiddlewareNode(action);
                _current = _current._next;
            }
        }

        public void UseMiddleWare<T>()
        {
            var type = typeof(T);

            if (!type.IsSubclassOf(typeof(MiddleWareBase)))
                throw new Exception("Class must inherit from MiddleWareBase");

            if (_root == null)
            {
                _root = (MiddleWareBase)Activator.CreateInstance(type);
                _current = _root;
            }
            else
            {
                _current._next = (MiddleWareBase)Activator.CreateInstance(type);
                _current = _current._next;
            }
        }

        public void UseMap(string map, RequestDelegate handler)
        {
            if (string.IsNullOrEmpty(map)) return;
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

            _root?.Invoke(context);
        }

        private bool _Map(WebContext context)
        {
            if (context == null) throw new Exception();

            foreach (var map in _maps)
            {
                var path = context.Request.path;
                if (string.Equals(map.Key, path, StringComparison.CurrentCultureIgnoreCase))
                {
                    map.Value.Invoke(context);

                    return true;
                }
            }

            return false;
        }
    }
}
