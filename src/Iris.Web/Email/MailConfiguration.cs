namespace Iris.Web.Email
{
    public class MailConfiguration
    {
        public bool EnableSsl { get; set; }
        public int SmtpPort { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string SmtpServer { get; set; }
    }
}