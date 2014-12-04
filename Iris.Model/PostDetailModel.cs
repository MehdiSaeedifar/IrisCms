using System;

namespace Iris.Model
{
    public class PostDetailModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PostedDate { get; set; }
        public int VisitedCount { get; set; }
        public int LikeCount { get; set; }
        public int CommnetCount { get; set; }
    }
}