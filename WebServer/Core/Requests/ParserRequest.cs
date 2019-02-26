using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.Requests
{
    public class ParserRequest
    {
        private ParserRequest() { }

        public static Dictionary<String, String> GETQuerys(String query)
        {
            if (String.IsNullOrEmpty(query)) return default(Dictionary<String, String>);

            var Querys = new Dictionary<String, String>();
            var separateTokens = '&';
            var separateInToken = '=';

            foreach (var _query in query.Split(new char[] { separateTokens }))
            {
                var item = _query.Split(new char[] { separateInToken });
                Querys.Add(item[0], item[1]);
            }

            return Querys;
        }
    }
}
