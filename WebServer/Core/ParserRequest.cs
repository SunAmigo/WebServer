using System;
using System.Collections.Generic;

namespace WebServer.Core
{
    public class ParserRequest
    {
        private ParserRequest() { }

        public static Dictionary<string, string> Get(Request request)
        {
            if (request == null) throw new NullReferenceException(nameof(request));
            if (!request.path.Contains("?")) return default(Dictionary<string, string>);

            var queries = new Dictionary<string, string>();
            var path = request.path;
            var index = path.IndexOf("?");
            var _queries = request.path.Substring(index + 1);

            foreach (var query in _queries.Split('&'))
            {
                var item = query.Split('=');
                queries.Add(item[0], item[1]);
            }

            return queries;
        }

        public static Dictionary<string, string> Post(Request request)
        {
            return default(Dictionary<string, string>);
        }
    }
}
