
using System;
using Microsoft.Extensions.Configuration;

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace MovieManager.Data.Services
{

    public class EmailServiceMailTrap : IEmailService
    { 
        private readonly string USERNAME;
        private readonly string PASSWORD;
        private readonly string SMTPSERVER;
        private readonly int SMTPPORT;
        private readonly string EMAILADDRESS;
        
        public EmailServiceMailTrap(IConfiguration configuration)
        {  
            SMTPSERVER   = configuration["Mail:SmtpServer"];
            int.TryParse(configuration["Mail:SmtpPort"], out SMTPPORT);    
            EMAILADDRESS = configuration["Mail:EmailAddress"];        
            USERNAME     = configuration["Mail:AccountUserName"]; 
            PASSWORD     = configuration["Mail:AccountPassword"];  
        }
 
        // send email
        public bool Send(string to, string subject, string body, string from=null)
        {
            if (from == null) {
                from = EMAILADDRESS;
            }
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(from, from));
            mailMessage.To.Add(new MailboxAddress(to, to));
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart(TextFormat.Text)
            {
                Text = body
            };

            try {
                var smtpClient = new SmtpClient();
                smtpClient.Connect(SMTPSERVER, SMTPPORT, SecureSocketOptions.StartTls);
                smtpClient.Authenticate(USERNAME, PASSWORD);
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
                return true;
            }
            catch (Exception)
            {               
                return false;
            }
        }    

    }
}