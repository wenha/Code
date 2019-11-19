using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Web.Services
{
    public static class MessageService
    {
        public static Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.FromResult(0);
        }

        public static Task SendSmsAsync(string number, string message)
        {
            return Task.FromResult(0);
        }
    }
}
