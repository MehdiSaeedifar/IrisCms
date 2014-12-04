using System;
using System.Collections.Generic;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Model.LuceneModel;

namespace Iris.Servicelayer.Interfaces
{
    public interface IBookService
    {
        int Count { get; }
        void AddBook(Book book);
        void DeleteBookById(int id);
        void EditBook(Book book);
        IList<Book> GetAllBooks();
        Book GetBookById(int id);
        Book GetBook(Func<Book, bool> expression);
        IList<SliderImagesModel> GetSliderImages(int count);
        IList<LuceneBookModel> GetAllForLuceneIndex();
    }
}