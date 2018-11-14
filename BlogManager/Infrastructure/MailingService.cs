using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace BlogManager.Infrastructure
{
    public interface IMailingService
    {
        void SendEmail(string targetAddress, string subject, string emailBody, bool isBodyHtml);
        void SendVerificationCode(string targetAddress, string code);
    }

    public class MailingService : IMailingService
    {
        private SmtpClient _smtpClient;
        private readonly string _emailAddress = "blog.manager.app@gmail.com";
        private readonly string _password = "blogmanager1";

        public MailingService()
        {
            _smtpClient = new SmtpClient("smtp.gmail.com", 587);
            _smtpClient.EnableSsl = true;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new NetworkCredential(_emailAddress, _password);
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        public void SendEmail(string targetAddress, string subject, string emailBody, bool isBodyHtml)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(_emailAddress, "Blog Manager");
            mail.To.Add(new MailAddress(targetAddress));
            mail.Subject = subject;
            mail.IsBodyHtml = isBodyHtml;
            mail.Body = emailBody;

            _smtpClient.Send(mail);
        }

        public void SendVerificationCode(string targetAddress, string code)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(_emailAddress, "Blog Manager");
            mail.To.Add(new MailAddress(targetAddress));
            mail.Subject = "Password Reset Request";
            mail.IsBodyHtml = true;
            mail.Body = $"<div>" +
                    $"<h2>Password Reset - Verification Code:</h2>" +
                    $"<h2 style='color: green;'>{code}</h2>" +
                    $"<p style='margin-top: 10px;'>Please contact the administrator if you didn't claim for password reset</p>" +
                    $"</div>"; ;

            _smtpClient.Send(mail);
        }

    }
}