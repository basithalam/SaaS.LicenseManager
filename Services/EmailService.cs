using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using SaaS.LicenseManager.Models;

namespace SaaS.LicenseManager.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public void SendLicenseEmail(string toEmail, string licenseKey)
        {
            using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password)
            };

            var mail = new MailMessage(_settings.SenderEmail, toEmail)
            {
                Subject = "Your License Key",
                Body = $"Hello!\n\nHere is your license key:\n{licenseKey}\n\nThank you for registering!"
            };

            client.Send(mail);
        }
    }
}