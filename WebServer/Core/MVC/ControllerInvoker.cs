using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebServer.Core.Attributes;
using WebServer.Core.MVC.Result;

namespace WebServer.Core.MVC
{
    public static class ControllerInvoker
    {
        public delegate object CastToType(string value);

        public static void Invoke(String controller,String action,WebContext context)
        {
            var asm = context.Assembly;

            var suffixOfController = "Controller";
            var directoryControllers = ".Controllers.";          
            var _namespace = asm.FullName.Split(new char[] { ',' })[0];

            var fullType = _namespace + directoryControllers + controller + suffixOfController;

            Type Controller = asm.GetType(fullType, false, true);
            if (Controller == null)
            {
                ViewResult.Render(WebContext.TypeError._404, context, $"Not found controller {controller}");
                return;
            }

            var constructor = Controller.GetConstructor(new Type[] { typeof(WebContext) });
            if (constructor == null) throw new ApplicationException(nameof(constructor));
            var obj = constructor.Invoke(new object[] { context });


            var methods = Controller.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m=>String.Compare(m.Name,action,true)==0);
            if (methods == null)
            {
                ViewResult.Render(WebContext.TypeError._404, context, $"Not found method {action}");
                return;

            }
            try
            {
                var (method, args) = GetArguments(methods, action, context);
                var ret = method.Invoke(obj, args);
                if (method.ReturnType != typeof(void))
                {
                    var _action = ret as IActionResult;
                    if (_action != null)
                    {
                        _action.ExecuteResult(context);
                    }
                    else
                    {
                        context.Response.Write(ret.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ViewResult.Render(WebContext.TypeError._404, context,ex.Message);
                return;
            }
           
        }

        private static (MethodInfo,object[]) GetArguments(IEnumerable<MethodInfo> methods,String action,WebContext context)
        {
            var method = default(MethodInfo);
            List<object> args =null;

            var stringQuery = context.Request.query;
            var dictionaryQuery = default(Dictionary<String, String>);

            if (String.IsNullOrEmpty(stringQuery))
            {
                method = methods.FirstOrDefault(m => m.GetParameters().Count() == 0);
                if (method == null)
                    throw new Exception($"method {action} Not found");
                return (method, args?.ToArray());
            }

            switch (context.Request.type)
            {
                case WebContext.TypeRequest.GET:
                    
                    dictionaryQuery = context.Request.GETRequest.Querys;
                    
                    methods = methods.Where(m => !(m.GetParameters().Count() == 0)
                    && m.GetCustomAttribute(typeof(POSTAttribute)) == null);
                    break;

                case WebContext.TypeRequest.POST:
                    dictionaryQuery = context.Request.POSTRequest.Form;

                    methods = methods.Where(m => !(m.GetParameters().Count() == 0)
                    && m.GetCustomAttribute(typeof(GETAttribute)) == null);
                    break;
            }
            
            foreach (var _method in methods)
            {
                args = new List<object>();
                method = _method;
                var querys = GetQueryQueue(dictionaryQuery);

                foreach (var parameter in _method.GetParameters())
                {
                    var ifArgumentNotValid = false;

                    var type = parameter.ParameterType;
                    var lastParameter = _method.GetParameters().LastOrDefault();

                    var ifPrimitiveOrString = parameter.ParameterType.IsPrimitive || parameter.ParameterType.IsValueType || parameter.ParameterType == typeof(String);
                    if (ifPrimitiveOrString)
                    {
                        ifArgumentNotValid = ProcessingPrimitive(querys,type,parameter, args);
                    }
                    else
                    {
                        ifArgumentNotValid = ProcessingClass(type, querys, args);

                    }

                    if (ifArgumentNotValid)
                    {
                        method = null;
                        args.Clear();
                        break;
                    }

                    var ifLastParameter = parameter == lastParameter && querys.Count == 0;
                    if (ifLastParameter)
                    {
                        return (method, args?.ToArray());
                    }
                }
            }

            if(method == null)
                    throw new Exception($"method {action} doesn't exist or arguments not valid");

            return default((MethodInfo,object[]));

        }

        private static  Queue<(String, String)> GetQueryQueue(Dictionary<String, String> Querys)
        {
            if (Querys == null) return default(Queue<(String, String)>);

            var queue = new Queue<(String, String)>();
            foreach (var item in Querys)
            {
                queue.Enqueue((item.Key,item.Value));
            }
            return queue;
        }

        private static List<(String, String)> GetObjectProperties(int countParameters, Queue<(String, String)> querys)
        {
            var list = new List<(String, String)>();
            for (int i = 0; i < countParameters; i++)
            {
                list.Add(querys.Dequeue());
            }
            return list;
        }

        private static object CastTo(Type type,string value)
        {
            var method = type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m=>m.Name=="Parse" && m.GetParameters().Length==1)
                .FirstOrDefault();
            return method.Invoke(null,new object [] {value});
        }

        private static object ConvertParameter(string value, Type _type)
        {
            if (_type == typeof(string))
            {
                return value;
            }

            return CastTo(_type, value);
        }

        private static bool ProcessingPrimitive(Queue<(String, String)> querys, Type type, ParameterInfo parameter, List<object> args)
        {
            var (name, value) = querys.Peek();
            if (string.Compare(parameter.Name, name, true) == 0)
            {
                var newValue = ConvertParameter(value, type);
                args.Add(newValue);
                querys.Dequeue();

                return false;

            }

            return true;
        }

        private static bool ProcessingClass(Type type, Queue<(String, String)> querys, List<object> args)
        {
            var obj = Activator.CreateInstance(type);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetIndexParameters().Length == 0);
            var parameters = GetObjectProperties(properties.Count(), querys);

            foreach (var propertyInfo in properties)
            {
                var arg = parameters.Where(p => string.Compare(p.Item1, propertyInfo.Name, true) == 0)
                    .FirstOrDefault();

                if (arg.Item1 == default(string) && arg.Item2 == default(string))
                {
                    return true;
                }
                var (name, value) = arg;
                var _type = propertyInfo.PropertyType;
                var newValue = ConvertParameter(value, _type);
                propertyInfo.SetValue(obj, newValue);
            }
            args.Add(obj);

            return false;
        }

    }
}
