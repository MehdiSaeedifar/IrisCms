using System;
using System.Collections.Generic;

namespace Iris.Model
{
    public class PostCommentsList
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public DateTime? AddedDate { get; set; }
        public PostCommentsList Parent { get; set; }
        public IList<PostCommentsList> Children { get; set; }
    }
}