using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebServer.Core.MVC
{
    static class ParserRoute
    {
        public static  (String, String) GetMVC(RouteMVC route, String path)
        {
            var controller = String.Empty;
            var action = String.Empty;

            var pattern = route.Template;

            pattern = pattern.Replace("{controller}", "[a-z]+")
                             .Replace("{action}", "[a-z]+");

            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(path);
            var fullPath = matches[0].Value.Split(new char[] { '/' });
            (controller, action) = (fullPath[0],fullPath[1]);

            return (controller,action);
        }
        public static (String, String) GetDefaultMVC(RouteMVC route)
        {
            var controller = String.Empty;
            var action = String.Empty;

            var defaultRoute = route.Default;
            var template = defaultRoute.Split(new char[] {'/'});

            controller= template[0].Split(new char[] { '=' })[1];
            action    = template[1].Split(new char[] { '=' })[1];

            return (controller, action);
        }
    }
}
