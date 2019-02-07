using System;
using System.Linq;
using System.Collections.Generic;
using WebServer.Core;

namespace WebServer.Core
{
    public class Request
    {
        public Dictionary<String, String> Querys; //GET
        public Dictionary<String, String> Form;   //Post

        public String type { get; set; }
        public String path { get; set; }
        public String host { get; set; }
        public String userAgent { get; set; }
        public String language { get; set; }

        private Request(String type, String path, String host, String userAgent, String language)
        {
            this.type = type;
            this.path = path;
            this.host = host;
            this.userAgent = userAgent;
            this.language = language;

            if (type.Contains("GET"))
            {
                Querys = ParserRequest.GET(this);
            }
        }

        public static Request GetRequest(String msg)
        {
            if (String.IsNullOrEmpty(msg)) return null;

            String[] tokents = msg.Split(new char[] { '\n' });

            //var type = (from tokentLine in tokents
            //            where tokentLine.Contains("HTTP")
            //            select tokentLine.Split(' ')[0].ToString())
            //            .FirstOrDefault();
            var type = "GET";

            var path = (from tokentLine in tokents
                        where tokentLine.Contains("HTTP")
                        select tokentLine.Split(' ')[1].ToString())
                        .FirstOrDefault()
                        .ToLower();

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
            var request = new Request(type, path, host, userAgent, language);



            #region  Console
            Logger.Log($"Request:\n{request}");
            Console.WriteLine();
            #endregion

            return request;
        }
        public static Request PostRequest(String msg)
        {
            return default(Request);
        }
        public override string ToString()
        {
            return String.Format("Type: {0}\nPath: {1}\nHost: {2}\nUserAgent: {3}\nLanguage: {4}", type, path, host, userAgent, language);
        }
    }
}