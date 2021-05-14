using System;
using System.Collections.Generic;
using System.Linq;
using EFCoreSecondLevelCacheInterceptor;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Iris.Servicelayer.EFServices
{
    public class BookService : IBookService
    {
        private readonly DbSet<Book> _books;

        public BookService(IUnitOfWork uow)
        {
            _books = uow.Set<Book>();
        }

        public void AddBook(Book book)
        {
            _books.Add(book);
        }

        public void DeleteBookById(int id)
        {
            _books.Remove(_books.Find(id));
        }

        public void EditBook(Book book)
        {
            throw new NotImplementedException();
        }

        public IList<Book> GetAllBooks()
        {
            return _books.ToList();
        }

        public Book GetBookById(int id)
        {
            throw new NotImplementedException();
        }

        public Book GetBook(Func<Book, bool> expression)
        {
            return _books.Where(expression).FirstOrDefault();
        }

        public IList<SliderImagesModel> GetSliderImages(int count)
        {
            return _books.AsNoTracking().OrderByDescending(x => x.Id).Select(book => new SliderImagesModel
            {
                ImageDescription = book.Image.Description,
                PostId = book.Id,
                PostTitle = book.Post.Title,
                ImagePath = book.Image.Path,
                ImageTitle = book.Image.Title
            }).Take(count)
                .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromHours(1))
                .ToList();
        }

        public int Count
        {
            get { return _books.Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromHours(1)).Count(); }
        }

        public IList<LuceneBookModel> GetAllForLuceneIndex()
        {
            return _books.AsNoTracking().Select(book => new LuceneBookModel
            {
                Author = book.Author,
                Description = book.Description,
                ISBN = book.ISBN,
                Name = book.Name,
                PostId = book.Post.Id,
                Publisher = book.Publisher,
                Title = book.Post.Title
            }).ToList();
        }
    }
}