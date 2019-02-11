using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace WebServer.Core
{
    public class Response
    {
        private bool _connected = true;
        private readonly NetworkStream _stream;

        public string Status { get; set; } = "200 OK";
        public string Mime { get; set; } = "text/html";

        public Response(NetworkStream stream)
        {
            if (stream != null)
                _stream = stream;
        }

        public void Write(string data)
        {
            if (_connected) _write(data);
        }

        private void _write(string data)
        {
            var writer = new StreamWriter(_stream);
            var dataBytes = Encoding.Default.GetBytes(data);

            Logger.Log("Response:");
            Logger.Log(
                $"{WebHost.Version} {Status}{Environment.NewLine}Server: {WebHost.ServerName}{Environment.NewLine}Content-Language: {"ua"}{Environment.NewLine}Content-Type: {Mime}{Environment.NewLine}" +
                $"Accept-Range: bytes{Environment.NewLine}Content-Length:{dataBytes.Length}{Environment.NewLine}Connection: close{Environment.NewLine}");

            writer.WriteLine(
                $"{WebHost.Version} {Status}{Environment.NewLine}Server: {WebHost.ServerName}{Environment.NewLine}Content-Language: {"ua"}{Environment.NewLine}Content-Type: {Mime}{Environment.NewLine}" +
                $"Accept-Range: bytes{Environment.NewLine}Content-Length:{dataBytes.Length}{Environment.NewLine}Connection: close{Environment.NewLine}");
            writer.Flush();

            try
            {
                _stream.Write(dataBytes, 0, dataBytes.Length);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error{ex.Message}");
                Console.WriteLine();
            }
            finally
            {
                _connected = false;
            }
        }
    }
}