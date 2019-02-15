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
        public IActionResult index()
        {
            //context.Response.Write("Index invoke");   
            //return new HtmlResult("index 0");
            //return new ViewResult("index");
            return new Redirect();

        }

        public IActionResult index(Int32 id)
        {
            //context.Response.Write("Index invoke");   
            //return new HtmlResult($"{id}");
            return new ViewResult("index");
        }
        //public IActionResult index(int id, string msg)
        //{
        //    context.Response.Write("Index invoke");
        //    return new HtmlResult($"{msg} - {id}");
        //}
        //public String index()
        //{
        //    context.Response.Write("Index invoke");
        //    return "String Action";
        //}

    }
}
