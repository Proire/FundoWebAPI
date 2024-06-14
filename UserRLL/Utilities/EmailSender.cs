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
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(EmailDTO dto)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = dto.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_configuration["EmailSettings:SmtpServer"], Convert.ToInt32(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration["EmailSettings:SenderEmail"], Environment.GetEnvironmentVariable("MailPassword")); 
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
