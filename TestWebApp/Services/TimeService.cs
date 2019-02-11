using System;

namespace TestWebApp.Services
{
    internal class TimeService
    {
        public string GetTime()
        {
            return DateTime.Now.ToString("hh:mm:ss");
        }
    }
}
