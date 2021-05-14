using System;

namespace Iris.Utilities.Mail
{
    public enum SendingMailResult
    {
        Successful,
        Faild
    }

    public static class EMail
    {
        public static string SmtpServer { get; set; }
        public static int SmtpPort { get; set; }
        public static bool EnableSsl { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string From { get; set; }

        public static SendingMailResult Send(string to, string subject, string body, ref string sendingResultError)
        {
            // TODO
            //WebMail.SmtpServer = SmtpServer;
            //WebMail.SmtpPort = SmtpPort;
            //WebMail.EnableSsl = EnableSsl;
            //WebMail.UserName = UserName;
            //WebMail.Password = Password;
            //WebMail.SmtpUseDefaultCredentials = true;
            //WebMail.From = From;

            //try
            //{
            //    WebMail.Send(to, subject, body, From);
            //}
            //catch (Exception ex)
            //{
            //    sendingResultError = ex.Message;
            //    return SendingMailResult.Faild;
            //}
            return SendingMailResult.Successful;
        }
    }
}