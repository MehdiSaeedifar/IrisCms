using Iris.DomainClasses.Entities;

namespace Iris.Model.AdminModel
{
    public class AddPostModel
    {
        public AddPostModel()
        {
            DownloadLinks = new DownloadLinks();
        }

        public virtual int PostId { get; set; }
        public virtual string PostTitle { get; set; }
        public virtual string PostKeyword { get; set; }
        public virtual string PostDescription { get; set; }
        public virtual PostStatus PostStatus { get; set; } // visible hidden draft archive
        public virtual bool? PostCommentStatus { get; set; }

        public virtual string PostBody { get; set; }

        public int[] LabelId { get; set; }
        public Book Book { get; set; }
        public BookImage BookImage { get; set; }
        public string UserName { get; set; }
        public DownloadLinks DownloadLinks { get; set; }
    }

    public class DownloadLinks
    {
        public DownloadLink DownloadLink1 { get; set; }
        public DownloadLink DownloadLink2 { get; set; }
        public DownloadLink DownloadLink3 { get; set; }
        public DownloadLink DownloadLink4 { get; set; }
    }
}