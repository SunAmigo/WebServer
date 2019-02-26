using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.Requests
{
    public class POST
    {
        public Dictionary<String, String> Form; 

        public string Referer       { get; set; }
        public string ContentType   { get; set; }
        public string ContentLength { get; set; }


        public POST(string referer, string contentType, string contentLength, Dictionary<String, String> form)
        {
            this.Referer = referer;
            this.ContentType= contentType;
            this.ContentLength = contentLength;
            Form = form;
        }

        public override string ToString()
        {
            return String.Format($"Referer: {Referer}{Environment.NewLine}ContentType: {ContentType}{Environment.NewLine}ContentLength: {ContentLength}{Environment.NewLine}");
        }
    }
}
