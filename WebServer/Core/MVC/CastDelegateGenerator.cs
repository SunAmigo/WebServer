using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using WebServer.Core.MVC;
using static WebServer.Core.MVC.ControllerInvoker;

namespace WebServer.Core.MVC
{
    public class CastDelegateGenerator
    {
        private ILGenerator il;

        public CastToType Generate(Type type)
        {
            var dynamicMethod = new DynamicMethod("__DynamicCast",typeof(object),new[] { typeof(string) },type.Module);
            il = dynamicMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);

            var value = il.DeclareLocal(type);

            il.Emit(OpCodes.Castclass, type);

            il.Emit(OpCodes.Stloc,value);

            il.Emit(OpCodes.Ldarg,value);

            il.Emit(OpCodes.Ret);

            return (CastToType)dynamicMethod.CreateDelegate(typeof(CastToType));
        }
    }
}
