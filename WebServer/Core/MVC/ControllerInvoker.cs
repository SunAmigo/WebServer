using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core.MVC.Result;

namespace WebServer.Core.MVC
{
    static class ControllerInvoker
    {
        public static void Invoke(String controller,String action,WebContext context)
        {
            var asm = context.Assembly;
            var assembly = asm.FullName.Split(new char[] { ',' })[0];
            var fullType = assembly + ".Controllers." + controller;

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
                .Where(m=>String.Compare(m.Name,action)==0);
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
                    if (parameter.ParameterType.IsPrimitive)
                    {
                        var (name, value) = querys.Peek();
                        if (String.Compare(parameter.Name, name) == 0)
                        {
                            //...... value cast to parameter.ParameterType!
                            args.Add(value);
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
                       //class..
                    }       
                }
            }
            return (method, args?.ToArray());
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

    }
}
