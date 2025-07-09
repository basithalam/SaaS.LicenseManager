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

        public async Task SendLicenseEmail(string toEmail, string licenseKey, string expiryDate)
        {
            using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password)
            };

            var mail = new MailMessage(_settings.SenderEmail, toEmail)
            {
                Subject = "Your License Key",
                Body = $"Hello User!\n\nHere is your license key: {licenseKey}\nYour license is valid until: {expiryDate}\n\nThank you for registering!"
            };

            await client.SendMailAsync(mail);
        }

        public async Task SendLicenseValidityUpdateEmail(string toEmail, string licenseKey, DateTime newExpireDate)
        {
            using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password)
            };

            var mail = new MailMessage(_settings.SenderEmail, toEmail)
            {
                Subject = "Your License Validity Has Been Updated",
                Body = $"Hello User!\n\nYour license (Key: {licenseKey}) validity has been updated.\nYour new license expiry date is: {newExpireDate.ToShortDateString()}\n\nThank you!"
            };

            await client.SendMailAsync(mail);
        }
    }
}