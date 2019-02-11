using System;

namespace WebServer.Core
{
    public static class Logger
    {
        public static void Log(string data)
        {
            var tmp = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(data);
            Console.ForegroundColor = tmp;
        }

        public static void Error(string data)
        {
            var tmp = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(data);
            Console.ForegroundColor = tmp;
        }
    }
}
