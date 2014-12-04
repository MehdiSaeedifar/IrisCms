using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Model.EmailModel
{
    public class CommentReplyNotificationModel : EmailModelBase
    {
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string CommentText { get; set; }
        public string CommentLink { get; set; }

    }
}
