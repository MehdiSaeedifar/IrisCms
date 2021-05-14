using System;

namespace Iris.Model.AdminModel
{
    public class UserDataTableModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool? IsBaned { get; set; }
        public string RoleDescription { get; set; }
        public string AvatarPath { get; set; }
        public int CommentCount { get; set; }
        public int PostCount { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? RegisterDate { get; set; }
    }
}