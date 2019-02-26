using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebServer.Core.MVC
{
    static class RouteValidator
    {
        public static bool IsValid(RouteMVC route,String path)
        {
            var pattern= route.Template.Replace("{controller}","[a-z]+")
                .Replace("{action}", "[a-z]+");

            if (Regex.IsMatch(path, pattern, RegexOptions.IgnoreCase))
            {
                return true;
            }

            return false;

        }
    }
}
