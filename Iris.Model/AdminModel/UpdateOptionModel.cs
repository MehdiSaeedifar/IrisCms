namespace Iris.Model.AdminModel
{
    public class UpdateOptionModel
    {
        public string SiteUrl { get; set; }
        public string BlogName { get; set; }
        public string BlogKeywords { get; set; }
        public string BlogDescription { get; set; }
        public bool UsersCanRegister { get; set; }
        public string AdminEmail { get; set; }
        public bool CommentsNotify { get; set; }
        public string MailServerUrl { get; set; }
        public string MailServerLogin { get; set; }
        public string MailServerPass { get; set; }
        public int? MailServerPort { get; set; }
        public bool CommentModeratingStatus { get; set; }
        public int PostPerPage { get; set; }
    }
}