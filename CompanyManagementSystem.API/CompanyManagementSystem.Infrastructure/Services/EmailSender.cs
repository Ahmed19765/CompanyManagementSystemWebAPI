using CompanyManagementSystem.Application.Interfaces.Services.Communication;
using CompanyManagementSystem.Application.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            
            // Connect to Gmail SMTP using STARTTLS (port 587)
            await client.ConnectAsync(
                _emailSettings.SmtpServer, 
                _emailSettings.Port, 
                MailKit.Security.SecureSocketOptions.StartTls);

            // Authenticate using sender credentials (usually SenderEmail and App Password)
            await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);

            // Send the email
            await client.SendAsync(emailMessage);

            // Disconnect and clean up
            await client.DisconnectAsync(true);
        }
    }
}
