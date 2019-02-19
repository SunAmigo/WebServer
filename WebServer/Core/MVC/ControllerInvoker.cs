using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core.MVC.Result;

namespace WebServer.Core.MVC
{
    public static class ControllerInvoker
    {
        public delegate object CastToType(string value);

        public static void Invoke(String controller,String action,WebContext context)
        {
            var asm = context.Assembly;
            var _namespace = asm.FullName.Split(new char[] { ',' })[0];
            var fullType = _namespace + ".Controllers." + controller;

            Type Controller = asm.GetType(fullType, false, true);
            if (Controller == null) throw new ApplicationException($"Not found controller : {controller}");

            var constructor = Controller.GetConstructor(new Type[] { typeof(WebContext) });
            if (constructor == null) throw new ApplicationException(nameof(constructor));
            var obj = constructor.Invoke(new object[] { context });

            //MethodInfo method = Controller.GetMethods(action);//!!!!!! case
            //if (method == null) throw new ApplicationException($"Not found method of controller : {action}");
            //var ret = method.Invoke(obj, null);
            //page not found View()

            var methods = Controller.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m=>String.Compare(m.Name,action,true)==0);
            if(methods==null) throw new ApplicationException($"Not found methods like as : {action}");
            var (method, args) = GetArguments(methods,action,context);
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

        private static (MethodInfo,object[]) GetArguments(IEnumerable<MethodInfo> methods,String action,WebContext context)
        {
            var method = default(MethodInfo);
            List<object> args =null;
            var stringQuery = context.Request.query;
            var dictionaryQuery = context.Request.Querys;

            if (String.IsNullOrEmpty(stringQuery))
            {
                method = methods.Where(m => m.GetParameters().Count() == 0).FirstOrDefault();
                if (method == null)
                {
                    throw new ApplicationException($"method {action} Not found ");
                    //...
                }
                return (method, args?.ToArray());
            }

            foreach (var _method in methods.Where(m=>!(m.GetParameters().Count()==0)))
            {
                args = new List<object>();
                method = _method;
                var querys = GetQueryQueue(dictionaryQuery);

                foreach (var parameter in _method.GetParameters())
                {
                    var type = parameter.ParameterType;
                    var lastParameter = _method.GetParameters().LastOrDefault();

                    if (parameter.ParameterType.IsPrimitive || parameter.ParameterType.IsValueType || parameter.ParameterType==typeof(String))
                    {
                        var (name, value) = querys.Peek();
                        if (String.Compare(parameter.Name, name,true) == 0)
                        {
                            //var _CastToType = new CastDelegateGenerator().Generate(parameter.ParameterType);
                            //var newValue = _CastToType(value);

                            var newValue = ConvertParameter(value,type);
                            args.Add(newValue);
                            querys.Dequeue();
                        }
                        else
                        {
                            method = null;
                            args.Clear();
                            break;
                        }
                    }
                    else
                    {
                        //else class..
                        var obj = Activator.CreateInstance(type);
                        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(p=>p.GetIndexParameters().Length==0);
                        var parameters = GetObjectProperties(properties.Count(),querys);

                        foreach (var propertyInfo in properties)
                        {
                            var arg = parameters.Where(p => string.Compare(p.Item1, propertyInfo.Name, true) == 0)
                                .FirstOrDefault();

                            if (arg.Item1==default(string) && arg.Item2==default(string))
                            {
                                method = null;
                                args.Clear();
                                break;
                            }
                            var (name, value) = arg;
                            var _type = propertyInfo.PropertyType;
                            var newValue = ConvertParameter(value,_type);
                            propertyInfo.SetValue(obj,newValue);
                        }
                        args.Add(obj);
                    }
                    if (parameter == lastParameter && querys.Count==0)
                    {
                        return (method, args?.ToArray());
                    }
                }
            }
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
            MethodInfo method = type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m=>m.Name=="Parse" && m.GetParameters().Length==1)
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
       
    }
}
