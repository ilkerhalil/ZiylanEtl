using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Mail;
using ZiylanEtl.Abstraction.Notification;

namespace ZiylanEtl.EmailNotification
{
    public class ZiylanMailer : INotification
    {
        private SmtpClient _smtpClient;
        public string Account { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string SenderName { get; set; }
        public string To { get; set; }
        public string Address { get; set; }
        public string Subject { get; set; }
        public int Port { get; set; }

        public void Send(string content)
        {
            _smtpClient = new SmtpClient(Address, Port);
            _smtpClient.Credentials = new NetworkCredential(Account, Password);
            _smtpClient.EnableSsl = false;
            MailMessage mailMessage = new MailMessage();
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.From = new MailAddress(From, SenderName, Encoding.UTF8);
            mailMessage.To.Add(To);
            mailMessage.Body = content;
            mailMessage.Subject = Subject;
            _smtpClient.Send(mailMessage);
        }
    }
}
