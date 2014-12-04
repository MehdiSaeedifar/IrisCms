using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iris.Web.Email
{
    public class CommentReplyEmailNotificationData
    {
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string CommentText { get; set; }
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public int CommentId { get; set; }
        public string ToEmail { get; set; }
    }
}