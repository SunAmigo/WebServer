using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core;
using WebServer.Core.MVC.Result;

namespace TestWebApp.Controllers
{
    class Home
    {
        private WebContext context { get; set; }
        public Home(WebContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            //context.Response.Write("Index invoke");   
            //return "String Action";
            return new ViewResult("index");
        }

        public IActionResult Index(Int32 id)
        {
            return new HtmlResult($"id = {id}");
        }
        public IActionResult Index(int id, string msg)
        {
            return new HtmlResult($"msg: {msg} - id: {id}");
        }
        public IActionResult Student(Student std)
        {
            return new HtmlResult($"Student:  {std.Name} - {std.Age}");
        }

    }
}
