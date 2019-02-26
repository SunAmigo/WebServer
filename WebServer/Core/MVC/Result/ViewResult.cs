using System;
using System.Linq;
using System.Threading;
using System.IO;
using System.Diagnostics;
using static WebServer.Core.WebContext;

namespace WebServer.Core.MVC.Result
{
    public class ViewResult : IActionResult
    {
        private readonly string _view;
        private readonly string _controller;

        public ViewResult()
        {
            var st = new StackTrace(true);
            StackFrame sf = st.GetFrame(1);

            _view       = GetInvokeMethodName(sf.GetMethod().ToString());
            _controller = GetInvokeControllerName(sf.GetFileName().ToString());
        }

        public ViewResult(String view)
        {
            _view = view;

            var st = new StackTrace(true);
            StackFrame sf = st.GetFrame(1);

            _controller = GetInvokeControllerName(sf.GetFileName().ToString());
        }

        public void ExecuteResult(WebContext context)
        {
            var controllerWithoutEnd = _controller.Replace("Controller","");
            var fullPath = Directory.GetCurrentDirectory() + "/../../Views/"+ controllerWithoutEnd;
            var dir = new  DirectoryInfo(fullPath);
            var file = dir.GetFiles().Where(f => String.Compare(f.Name, _view,true) == 0 
                                              || String.Compare(f.Name, _view + ".html",true) == 0)
            .FirstOrDefault();

            if (file == null)
            {
                Render(TypeError._404,context);
            }

            var fs = file.OpenRead();
            var reader = new StreamReader(fs);
            var data = reader.ReadToEnd();

            context.Response.Write(data);

        }

        public static void Render(TypeError type,WebContext context,String info= "default")
        {
            var status = string.Empty;
            switch (type)
            {
                case TypeError._200: status = "200 OK";                 break;
                case TypeError._404: status = "404 Page Not Found";     break;
                case TypeError._405: status = "405 Method Not Allowed"; break;                
            }

            var fullpath = Directory.GetCurrentDirectory() + "/../../Views/Error/" + (int)type;
            var file = new FileInfo(fullpath);

            var fs = file.OpenRead();
            var reader = new StreamReader(fs);
            var data = reader.ReadToEnd();

            if (string.Compare(info, "default", true) != 0)
            {
                data = data.Replace("{{info}}", info);
            }
            else
            {
                data = data.Replace("{{info}}", "");
            }

            context.Response.Status = status;
            context.Response.Write(data);

        }

        private static string GetInvokeMethodName(string fullMethodName)
        {
            var nameWithArguments = fullMethodName.Split(new char[] {' '})[1];
            var indexOfBeginArgumentFunction = nameWithArguments.IndexOf("(");

            return nameWithArguments.Substring(0, indexOfBeginArgumentFunction);
        }

        private static string GetInvokeControllerName(string fullName)
        {
            return fullName.Split(new char[] { '\\' }).Last().Replace(".cs", "");
        }
    }

}
