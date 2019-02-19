using System;
using System.Linq;
using System.Collections.Generic;
using WebServer.Core;
using System.Net.Sockets;
using System.Net;

namespace WebServer.Core
{
    public class Request
    {
        public Dictionary<String, String> Querys; //GET
        public Dictionary<String, String> Form;   //Post

        public String type      { get; set; }
        public String path      { get; set; }
        public String query     { get; set; }
        public String host      { get; set; }
        public String userAgent { get; set; }
        public String language  { get; set; }
        public EndPoint from      { get; set; }

        public Request(String type, String path,String query, String host, String userAgent, String language)
        {
            this.type = type;
            this.path = path;
            this.query = query;
            this.host = host;
            this.userAgent = userAgent;
            this.language = language;
        }
        public override string ToString()
        {
            return String.Format($"From: {from}{Environment.NewLine}Type: {type}{Environment.NewLine}Path: {path}{Environment.NewLine}Query: {query}{Environment.NewLine}Host: {host}{Environment.NewLine}UserAgent: {userAgent}{Environment.NewLine}Language: {language}");
        }
    }
}