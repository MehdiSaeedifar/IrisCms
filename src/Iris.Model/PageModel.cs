using System;

namespace Iris.Model
{
    public class PageModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string Body { get; set; }

        public string Keyword { get; set; }

        public string Description { get; set; }

        public int VisitedCount { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

        public string UserName { get; set; }
    }
}
