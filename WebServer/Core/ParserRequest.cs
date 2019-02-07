using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core
{
    public class ParserRequest
    {
        private ParserRequest() { }

        public static Dictionary<String, String> GET(Request request)
        {
            if (request == null) throw new NullReferenceException();
            if (!request.path.Contains("?")) return default(Dictionary<String, String>);

            var Querys = new Dictionary<String, String>();

            var path = request.path;
            var index = path.IndexOf("?");
            var _querys = request.path.Substring(index + 1);

            foreach (var query in _querys.Split(new char[] { '&' }))
            {
                var item = query.Split(new char[] { '=' });
                Querys.Add(item[0], item[1]);
            }

            return Querys;
        }
        public static Dictionary<String, String> Post(Request request)
        {
            return default(Dictionary<String, String>);
        }
    }
}
