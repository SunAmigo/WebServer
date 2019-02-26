using System;
using System.Collections.Generic;
using WebServer.Core.MiddleWare;
using WebServer.Core.MVC;


namespace WebServer.Core.Configuration
{
    public class ApplicationBuilder
    {
        private static readonly ApplicationBuilder instance = new ApplicationBuilder();

        public delegate void RequestDelegate(WebContext contex);

        public MiddleWareBase _root;
        public MiddleWareBase _curent;
        private Dictionary<String, RequestDelegate> _maps;

        private RouteMVC _routeMVC;

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
        public void UseMVC(String name,String template,String _default)
        {
            _routeMVC = new RouteMVC(name, template, _default);
        }

        public void StartMiddleware(WebContext context)
        {
            if (_routeMVC != null) _MVC(context);

            if (context.Request.type == WebContext.TypeRequest.GET)
            {
                _Map(context);
                _Use(context);
            }
        }
        private void _Use(WebContext context)
        {
            if (context == null) throw new NullReferenceException(nameof(context));
            _root?.Invoke(context);
        }
        private void  _Map(WebContext context)
        {
            if (context == null) throw new Exception();

            foreach (var map in _maps)
            {
                var path = context.Request.path;
                if (String.Equals(map.Key, path, StringComparison.CurrentCultureIgnoreCase))
                {
                    map.Value.Invoke(context);
                }
            }
        }

        private void _MVC(WebContext context)
        {
            var (controller, action) = default((String,String));
            var path = context.Request.path;
            if (String.Compare(path, "/") == 0)
            {
                 (controller, action) = ParserRoute.GetDefaultMVC(_routeMVC);
            }
            else if (RouteValidator.IsValid(_routeMVC, path))
            {
                 (controller, action) = ParserRoute.GetMVC(_routeMVC, path);
            }
            ControllerInvoker.Invoke(controller, action, context);
        }
    }
}
