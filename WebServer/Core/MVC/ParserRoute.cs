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
        private const char separate = '/';
        private const char separateInToken = '=';

        public static  (String, String) GetMVC(RouteMVC route, String path)
        {
            var controller = String.Empty;
            var action     = String.Empty;

            var pattern = route.Template.Replace("{controller}", "[a-z]+")
                             .Replace("{action}", "[a-z]+");

            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(path);

            var matchesString = String.Empty;
            foreach (Match match in matches)
            {
                matchesString += match.Value+ separate;
            }
            matchesString=matchesString.Remove(matchesString.Length-1);

            var fullPath = matchesString.Split(new char[] { separate });
            (controller, action) = (fullPath[0],fullPath[1]);

            return (controller,action);
        }
        public static (String, String) GetDefaultMVC(RouteMVC route)
        {
            var controller = String.Empty;
            var action = String.Empty;

            var defaultRoute = route.Default;
            var template = defaultRoute.Split(new char[] { separate });

            controller = template[0].Split(new char[] { separateInToken })[1];
            action     = template[1].Split(new char[] { separateInToken })[1];

            return (controller, action);
        }
    }
}
