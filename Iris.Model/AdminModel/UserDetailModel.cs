using System;

namespace Iris.Model.AdminModel
{
    public class UserDetailModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string IP { get; set; }
        public bool? IsBaned { get; set; }
        public string RoleName { get; set; }
        public int PostNumber { get; set; }
        public int CommentNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? BanedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastPasswordChange { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Major { get; set; }
        public string Description { get; set; }
        public string AvatarPath { get; set; }
    }
}