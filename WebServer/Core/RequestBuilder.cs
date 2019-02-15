using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core
{
    public static class RequestBuilder
    {
        public static Request GetRequest(String msg)
        {
            if (String.IsNullOrEmpty(msg)) return null;

            String[] tokents = msg.Split(new char[] { '\n' });

            var type = "GET";

            var fullpath = (from tokentLine in tokents
                            where tokentLine.Contains("HTTP")
                            select tokentLine.Split(' ')[1].ToString())
                        .FirstOrDefault()
                        ?.ToLower();

            var path = String.Empty;
            var query = String.Empty;
            if (fullpath.Contains("?"))
            {
                var index = fullpath.IndexOf("?");
                query = fullpath.Substring(index + 1);
                path = fullpath.Substring(0, index);
            }
            else path = fullpath;

            var host = (from tokentLine in tokents
                        where tokentLine.Contains("Host")
                        select tokentLine.Split(':')[1] + ":" + tokentLine.Split(':')[2])
                       .FirstOrDefault();

            var userAgent = (from tokentLine in tokents
                             where tokentLine.Contains("User-Agent")
                             select tokentLine.Split(':')[1].ToString())
                             .FirstOrDefault();
            var language = (from tokentLine in tokents
                            where tokentLine.Contains("Accept-Language")
                            select tokentLine.Split(new char[] { ':', ';' })[1].ToString())
                            .FirstOrDefault();
            var request = new Request(type, path, query, host, userAgent, language);
            request.Querys = ParserRequest.GET(query);

            Logger.Log($"Request:{Environment.NewLine}{request}");
            Console.WriteLine();

            return request;
        }
        public static Request PostRequest(String msg)
        {
            return default(Request);
        }
    }
}
