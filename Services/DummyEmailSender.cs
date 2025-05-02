using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;

namespace mercharteria.Services
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine("Email simulado:");
            Console.WriteLine($"Para: {email}");
            Console.WriteLine($"Asunto: {subject}");
            Console.WriteLine($"Contenido: {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}
