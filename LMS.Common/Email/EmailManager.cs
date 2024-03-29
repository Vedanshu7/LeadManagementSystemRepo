﻿using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace LMS.Common.Email
{
    public class EmailManager
    {
        public static void SendEmail(string From, string Subject, string Body, string To, string UserID, string Password, string SMTPPort, string Host)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress(From);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient
            {
                Host = Host,
                Port = Convert.ToInt16(SMTPPort),
                Credentials = new NetworkCredential(UserID, Password),
                EnableSsl = false
            };
            smtp.Send(mail);
        }

        public static void AppSettings(out string UserID, out string Password, out string SMTPPort, out string Host)
        {
            UserID = ConfigurationManager.AppSettings.Get("UserID");
            Password = ConfigurationManager.AppSettings.Get("Password");
            SMTPPort = ConfigurationManager.AppSettings.Get("SMTPPort");
            Host = ConfigurationManager.AppSettings.Get("Host");
        }
    }
}
