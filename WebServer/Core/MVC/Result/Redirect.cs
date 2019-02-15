using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.MVC.Result
{
    public class Redirect : IActionResult
    {
        private readonly String _controller;
        private readonly String _action;

        public Redirect(String controller="Home",String action="index")
        {
            _controller = controller;
            _action = action;
        }

        public void ExecuteResult(WebContext context)
        {
            //StackOverFlow!!!
            ControllerInvoker.Invoke(_controller, _action, context);
        }
    }
}
