using System.Web.Mvc;

namespace Iris.Model.AdminModel
{
    public class AddPageModel
    {
        public virtual string Title { get; set; }

        [AllowHtml]
        public virtual string Body { get; set; }

        public virtual string Keyword { get; set; }
        public virtual string Description { get; set; }
        public virtual string Status { get; set; }
        public virtual bool CommentStatus { get; set; }
        public virtual int? Order { get; set; }
        public virtual int? ParentId { get; set; }
    }
}