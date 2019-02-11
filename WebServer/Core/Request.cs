using System;
using System.Linq;
using System.Collections.Generic;

namespace WebServer.Core
{
    public class Request
    {
        public Dictionary<string, string> Queries; //GET
        public Dictionary<string, string> Form;   //Post

        public string type { get; set; }
        public string path { get; set; }
        public string host { get; set; }
        public string userAgent { get; set; }
        public string language { get; set; }

        private Request(string type, string path, string host, string userAgent, string language)
        {
            this.type = type;
            this.path = path;
            this.host = host;
            this.userAgent = userAgent;
            this.language = language;

            if (type.Contains("GET"))
            {
                Queries = ParserRequest.Get(this);
            }
        }

        public static Request GetRequest(string msg)
        {
            if (string.IsNullOrEmpty(msg)) return null;

            var tokens = msg.Split(new char[] { '\n' });

            //var type = (from tokentLine in tokents
            //            where tokentLine.Contains("HTTP")
            //            select tokentLine.Split(' ')[0].ToString())
            //            .FirstOrDefault();
            var type = "GET";

            var path = (from tokenLine in tokens
                        where tokenLine.Contains("HTTP")
                        select tokenLine.Split(' ')[1])
                        .FirstOrDefault()
                        ?.ToLower();

            var host = (from tokenLine in tokens
                        where tokenLine.Contains("Host")
                        select tokenLine.Split(':')[1] + ":" + tokenLine.Split(':')[2])
                       .FirstOrDefault();

            var userAgent = (from tokenLine in tokens
                             where tokenLine.Contains("User-Agent")
                             select tokenLine.Split(':')[1].ToString())
                             .FirstOrDefault();
            var language = (from tokenLine in tokens
                            where tokenLine.Contains("Accept-Language")
                            select tokenLine.Split(new char[] { ':', ';' })[1].ToString())
                            .FirstOrDefault();
            var request = new Request(type, path, host, userAgent, language);

            Logger.Log($"Request:\n{request}");
            Console.WriteLine();

            return request;
        }

        public static Request PostRequest(string msg)
        {
            return default(Request);
        }

        public override string ToString()
        {
            return $"Type: {type}\nPath: {path}\nHost: {host}\nUserAgent: {userAgent}\nLanguage: {language}";
        }
    }
}