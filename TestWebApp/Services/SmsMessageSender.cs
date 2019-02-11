using TestWebApp.Services.Interfaces;

namespace TestWebApp.Services
{
    internal class SmsMessageSender : IMessageSender
    {
        public string Send()
        {
            return "Sent by SMS";
        }
    }
}
