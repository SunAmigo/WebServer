using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.MVC.Result
{
    public interface IActionResult
    {
        void ExecuteResult(WebContext context);
    }
}
