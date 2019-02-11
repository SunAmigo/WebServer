using TestWebApp.Services.Interfaces;

namespace TestWebApp.Services
{
    internal sealed class EmailMessageSender : IMessageSender
    {
        public string Send()
        {
            return "Sent by Email";
        }
    }
}
