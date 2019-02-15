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

        public static Dictionary<String, String> GET(String query)
        {
            if (String.IsNullOrEmpty(query)) return default(Dictionary<String, String>);

            var Querys = new Dictionary<String, String>();
    
            foreach (var _query in query.Split(new char[] { '&' }))
            {
                var item = _query.Split(new char[] { '=' });
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
