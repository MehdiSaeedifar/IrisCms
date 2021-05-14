using System;

namespace Iris.Model
{
    public abstract class CommentDetailModelBase
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
        public int LikeCount { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}