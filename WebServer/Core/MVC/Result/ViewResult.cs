using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace WebServer.Core.MVC.Result
{
    public class ViewResult : IActionResult
    {
        private readonly String _view;
        public ViewResult(String view)
        {
            _view = view;
        }

        public void ExecuteResult(WebContext context)
        {
            var fullPath = Thread.GetDomain().BaseDirectory + "/../../View";
            var dir = new System.IO.DirectoryInfo(fullPath);
            var file = dir.GetFiles().Where(f => String.Compare(f.Name, _view) == 0 
            || String.Compare(f.Name, _view + ".html") == 0)
            .FirstOrDefault();
            if (file == null)
            {
                throw new ApplicationException($"Not found View : {file}");
            }

            var fs = file.OpenRead();
            var reader = new StreamReader(fs);
            var data = reader.ReadToEnd();

            context.Response.Write(data);

        }
    }
}
