using AuthService.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Email
{
    public class GmailEmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public GmailEmailService(
            IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendResetPasswordEmailAsync(string email, string token)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse("binb95982@gmail.com"));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Reset Password";

            message.Body = new TextPart("plain")
            {
                Text = $"Your reset token: {token}"
            };

            using var smtp = new SmtpClient();

            // 🔥 FIX SSL
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await smtp.ConnectAsync(
                "smtp.gmail.com",
                587,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                "binb95982@gmail.com",
                "axze rnsv ydho vlil");

            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
        }
    }
}
