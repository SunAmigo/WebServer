using System;
using System.Collections.Generic;
using System.Net;
using static WebServer.Core.WebContext;

namespace WebServer.Core.Requests
{
    public class Request
    {
        public GET  GETRequest  { get; set; }
        public POST POSTRequest { get; set; }

        public TypeRequest type { get; set; }
        public EndPoint from { get; set; }

        public String path { get; set; }
        public String query { get; set; }
        public String host { get; set; }
        public String userAgent { get; set; }
        public String language { get; set; }

        public Request(TypeRequest type,EndPoint from, String path, String query, String host, String userAgent, String language)
        {
            this.type = type;
            this.from = from;

            this.path = path;
            this.query = query;
            this.host = host;
            this.userAgent = userAgent;
            this.language = language;
        }

        public void InitializeGET(Dictionary<String, String> querys)
        {
            GETRequest = new GET(querys);
        }

        public void InitializePOST(string referer, string contentType, string contentLength, Dictionary<String, String> form)
        {
            POSTRequest = new POST(referer,contentType,contentLength,form);
        }

        public override string ToString()
        {
            var strView =  String.Format($"From: {from}{Environment.NewLine}Type: {type}{Environment.NewLine}Path: {path}{Environment.NewLine}Query: {query}{Environment.NewLine}Host: {host}{Environment.NewLine}UserAgent: {userAgent}{Environment.NewLine}Language: {language}");
            switch (type)
            {
                case TypeRequest.POST: strView += POSTRequest; break;
                default: break;
            }

            return strView;
        }


    }
}