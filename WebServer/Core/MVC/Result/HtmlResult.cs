using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.MVC.Result
{
    public class HtmlResult :  IActionResult
    {
        string htmlCode;
        public HtmlResult(string html)
        {
            htmlCode = html;
        }
        public void ExecuteResult(WebContext context)
        {
            string fullHtmlCode = "<!DOCTYPE html><html><head>";
            fullHtmlCode += "<meta charset=utf-8 />";
            fullHtmlCode += "</head> <body>";
            fullHtmlCode += htmlCode;
            fullHtmlCode += "</body></html>";
            context.Response.Write(fullHtmlCode);
        }
    }
}
