using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MailKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using static System.Net.Mime.MediaTypeNames;
using UserModelLayer;
using Azure.Core;

namespace UserRLL.Utilities
{
    public class EmailSender 
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void SendEmail(EmailDTO dto)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = dto.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.SenderEmail, Environment.GetEnvironmentVariable("MailPassword")); // Use valid email and password
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
