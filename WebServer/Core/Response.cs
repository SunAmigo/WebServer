using System;
using System.IO;
using System.Net.Sockets;
using System.Text;


namespace WebServer.Core
{
    public class Response
    {
        private bool _canWrite = true;
        private NetworkStream _stream;

        public String Status { get; set; } = "200 OK";
        public String Mime   { get; set; } = "text/html";


        public Response(NetworkStream stream)
        {
            _stream = stream ?? throw new NullReferenceException(nameof(NetworkStream));
        }

        public void Write(String data)
        {
            if (_canWrite) _write(data);
        }
        private void _write(String data)
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
                _stream.Write(dataBytes, 0,dataBytes.Length);
            }
            catch (Exception ex)
            {
                Logger.Error($"Eror{ex.Message}");
                Console.WriteLine();
            }
            finally
            {
                _canWrite = false;
            }
        }
    }
}