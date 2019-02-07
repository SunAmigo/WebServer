using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WebServer.Core;

namespace WebServer.Core
{
    public class Response
    {
        private NetworkStream _stream;

        public String Status { get; set; } = "200 OK";
        public String Mime   { get; set; } = "text/html";

        private Boolean connected { get; set; } = true;

        public Boolean isConnected
        {
            get { return connected; }
        }

        public Response(NetworkStream stream)
        {
            if (stream != null)
                this._stream = stream;
        }

        public void Write(String data)
        {
            if (connected == true) _write(data);
            //Console.WriteLine();
        }
        private void _write(String data)
        {
            var writer = new StreamWriter(_stream);

            byte[] _data = Encoding.Default.GetBytes(data);

            #region  Console
            Logger.Log("Response:");
            Logger.Log(String.Format("{0} {1}\r\nServer: {2}\r\nContent-Language: {3}\r\nContent-Type: {4}\r\n" +
      "Accept-Range: bytes\r\nContent-Length:{5}\r\nConnection: close\r\n", WebHost.VERSION, Status, WebHost.SERVERNAME, "ua", Mime, _data.Length));
            #endregion

            writer.WriteLine(String.Format("{0} {1}\r\nServer: {2}\r\nContent-Language: {3}\r\nContent-Type: {4}\r\n" +
                "Accept-Range: bytes\r\nContent-Length:{5}\r\nConnection: close\r\n", WebHost.VERSION, Status, WebHost.SERVERNAME, "ua", Mime, _data.Length));
            writer.Flush();

            try
            {
                _stream.Write(_data, 0, _data.Length);
            }
            catch (Exception ex)
            {
                Logger.Error($"Eror{ex.Message}");
                Console.WriteLine();
            }
            finally
            {
                connected = false;
            }
        }


    }
}