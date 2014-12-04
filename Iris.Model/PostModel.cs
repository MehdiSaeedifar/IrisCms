using System;
using System.Collections.Generic;
using Iris.DomainClasses.Entities;

namespace Iris.Model
{
    public class PostModel
    {
        public int Id { get; set; }
        public string PostTitle { get; set; }
        public string BookName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public int LikeCount { get; set; }
        public ICollection<Label> Lables { get; set; }
        public ICollection<DownloadLink> DownloadLinks { get; set; }
        public string PostStatus { get; set; }
        public bool? CommentStatus { get; set; }
        public string PostBody { get; set; }
        public string BookBody { get; set; }
        public string BookPublisher { get; set; }
        public string BookAuthor { get; set; }
        public string BookIsbn { get; set; }
        public string BookDate { get; set; }
        public string BookPageCount { get; set; }
        public string BookLanguage { get; set; }
        public string ImageTitle { get; set; }
        public string ImageDescription { get; set; }
        public string ImagePath { get; set; }
        public int VisitedCount { get; set; }
        public int CommentsCount { get; set; }
        public string PostDescription { get; set; }
        public string PostKeywords { get; set; }
    }
}