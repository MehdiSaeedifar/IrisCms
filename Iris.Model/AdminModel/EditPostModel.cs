using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Iris.DomainClasses.Entities;

namespace Iris.Model.AdminModel
{
    public class EditPostModel
    {
        public EditPostModel()
        {
            Links = new DownloadLinks();
            DownloadLinks = new List<DownloadLink>();
            Labels = new List<Label>();
        }

        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostKeyword { get; set; }
        public string PostDescription { get; set; }
        public string PostStatus { get; set; } // visible hidden draft archive
        public bool? PostCommentStatus { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [AllowHtml]
        public string PostBody { get; set; }

        [ForeignKey("LabelsId")]
        public ICollection<Label> Labels { get; set; }

        public int[] LabelsId { get; set; }
        public Book Book { get; set; }
        public BookImage BookImage { get; set; }
        public ICollection<DownloadLink> DownloadLinks { get; set; }
        public DownloadLinks Links { get; set; }
        public User EditedByUser { get; set; }
    }
}