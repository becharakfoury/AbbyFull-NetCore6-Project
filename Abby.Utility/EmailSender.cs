using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace Abby.Utility
{
    public class EmailSender : IEmailSender
    {
        //Get sendgrid api key from : appsetings.json
        public string SendGridSecret { get; set; }
        public EmailSender(IConfiguration _config)
        {
            SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey");
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            ////Method #1 / using: MimeKit
            //var emailToSend = new MimeMessage();
            //emailToSend.From.Add(MailboxAddress.Parse("hello@dotnetmastery.com"));
            //emailToSend.To.Add(MailboxAddress.Parse(email));
            //emailToSend.Subject = subject;
            //emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };
            ////send email using Method #1
            //using (var emailClient = new SmtpClient())
            //         {
            //	emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            //	emailClient.Authenticate("dotnetmastery@gmail.com", "password must be placed");
            //	emailClient.Send(emailToSend);
            //         }
            //return Task.CompletedTask;

            //Method #2: Using Sendgrid
            var client = new SendGridClient(SendGridSecret);
            var from = new EmailAddress("hello@dotnetmastery.com", "Abby Food");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            return client.SendEmailAsync(msg);

        }
    }
}
