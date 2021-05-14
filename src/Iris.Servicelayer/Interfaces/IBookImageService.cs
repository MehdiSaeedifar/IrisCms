using System;
using System.Collections.Generic;
using Iris.DomainClasses.Entities;

namespace Iris.Servicelayer.Interfaces
{
    public interface IBookImageService
    {
        void AddImage(BookImage bookImage);
        void RemoveImage(int id);
        void EditImage(BookImage bookImage);
        BookImage GetImageById(int id);
        BookImage GetImage(Func<BookImage, bool> expression);
        IList<BookImage> GetAllImages();
    }
}