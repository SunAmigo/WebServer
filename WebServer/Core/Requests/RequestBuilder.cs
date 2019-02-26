using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static WebServer.Core.WebContext;

namespace WebServer.Core.Requests
{
    public static class RequestBuilder
    {
        public static Request InitializeRequest(String msg, TcpClient client,TypeRequest type)
        {
            if (String.IsNullOrEmpty(msg)) return null;

             const char newLine = '\n';
             String[] tokents = msg.Split(newLine);


            var (host, userAgent, language, fullpath) = DefaultParse(tokents);

            switch (type)
            {
                case TypeRequest.GET:
                    {
                        var (path, query) = GetRequest(tokents, fullpath);
                        var request = new Request(type, client.Client.RemoteEndPoint, path, query, host, userAgent, language);
                        var Querys = ParserRequest.GETQuerys(query);
                        request.InitializeGET(Querys);

                        return request;
                    }
                case TypeRequest.POST:
                    {
                        var (contentType, contentLength, referer, query) = PostRequest(tokents, client);
                        var request = new Request(type, client.Client.RemoteEndPoint, fullpath, query, host, userAgent, language);
                        var Querys = ParserRequest.GETQuerys(query);
                        request.InitializePOST(referer,contentType,contentLength,Querys);

                        return request;
                    }
                default: break;
            }
           
            return default(Request);

        }

        public static (string,string) GetRequest(String[] tokents, string fullpath)
        {
            var path = String.Empty;
            var query = String.Empty;

            var beginQueryTokens = "?";
            if (fullpath.Contains(beginQueryTokens))
            {
                var indexOfbeginQueryTokens = fullpath.IndexOf(beginQueryTokens);
                query = fullpath.Substring(indexOfbeginQueryTokens + 1);
                path = fullpath.Substring(0, indexOfbeginQueryTokens);
            }
            else path = fullpath;

            return (path,query);
        }

        public static (string, string, string, string) PostRequest(String[] tokents, TcpClient client)
        {

            var ContentType = (from tokentLine in tokents
                                where tokentLine.Contains("Content-Type")
                                select tokentLine.Split(':')[1].ToString())
                                .FirstOrDefault();

            if (ContentType.Contains("application/x-www-form-urlencoded"))
            {
                var ContentLength = (from tokentLine in tokents
                                     where tokentLine.Contains("ContentLength")
                                     select tokentLine.Split(':')[1].ToString().Trim())
                                .FirstOrDefault()
                                ?.ToLower();

                var Referer = (from tokentLine in tokents
                               where tokentLine.Contains("Referer")
                               select tokentLine.Split(' ')[1].ToString())
                                .FirstOrDefault()
                                ?.ToLower();

                var separateTokens = '&';
                var dataFromForm = tokents.LastOrDefault();
                var indexOfLasttoken = dataFromForm.LastIndexOf(separateTokens);
                var query = dataFromForm.Substring(0, indexOfLasttoken);

                return (ContentType, ContentLength, Referer, query);
            }

            return default((string, string, string, string));
        }

        private static (string, string, string, string) DefaultParse(String[] tokents)
        {
            var path = (from tokentLine in tokents
                            where tokentLine.Contains("HTTP")
                            select tokentLine.Split(' ')[1].ToString())
            .FirstOrDefault()
            ?.ToLower();

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

            return (host, userAgent, language, path);
        }
    }
}
