using System;

namespace Iris.Model.AdminModel
{
    public class CommentDataTableModel
    {
        public int Id { get; set; }
        public string AvatarPath { get; set; }
        public string Body { get; set; }
        public DateTime? AddDate { get; set; }
        public bool? IsApproved { get; set; }
        public int LikeCount { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int? AuthorId { get; set; }
        public String AuthorName { get; set; }
        public int? PostId { get; set; }
        public string PostTitle { get; set; }
    }
}