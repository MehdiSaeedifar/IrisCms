using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Servicelayer.Interfaces;

namespace Iris.Servicelayer.EFServices
{
    public class BookImageService : IBookImageService
    {
        private readonly IDbSet<BookImage> _images;

        public BookImageService(IUnitOfWork uow)
        {
            _images = uow.Set<BookImage>();
        }

        public void AddImage(BookImage bookImage)
        {
            _images.Add(bookImage);
        }

        public void RemoveImage(int id)
        {
            _images.Remove(_images.Find(id));
        }

        public void EditImage(BookImage bookImage)
        {
            throw new NotImplementedException();
        }

        public BookImage GetImageById(int id)
        {
            return _images.Find(id);
        }

        public BookImage GetImage(Func<BookImage, bool> expression)
        {
            return _images.Where(expression).FirstOrDefault();
        }

        public IList<BookImage> GetAllImages()
        {
            return _images.ToList();
        }
    }
}