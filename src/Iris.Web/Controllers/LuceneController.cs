//using Iris.Datalayer.Context;
//using Iris.DomainClasses.Entities;
//using System.Linq;
//using Iris.Servicelayer.Interfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace Iris.Web.Controllers
//{
//    public class LuceneController : Controller
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IBookSearch _bookSearch;

//        public LuceneController(
//            IUnitOfWork unitOfWork,
//            IBookSearch bookSearch
//        )
//        {
//            _unitOfWork = unitOfWork;
//            _bookSearch = bookSearch;
//        }

//        [Authorize(Roles = "admin")]
//        public virtual ActionResult ReIndex()
//        {

//            var books = _unitOfWork.Set<Post>()
//                .AsNoTracking()
//                .OrderByDescending(x => x.Id)
//                .Select(post => new LuceneBookModel
//                {
//                    Author = post.Book.Author,
//                    Description = post.Book.Description,
//                    ISBN = post.Book.ISBN,
//                    Name = post.Book.Name,
//                    PostId = post.Id,
//                    Publisher = post.Book.Publisher,
//                    Title = post.Title
//                })
//                .ToList();

//            foreach (var bookModel in books)
//            {
//                _bookSearch.AddUpdateLuceneIndex(new LuceneBookModel
//                {
//                    Author = bookModel.Author,
//                    Description = bookModel.Description,
//                    ISBN = bookModel.ISBN,
//                    Name = bookModel.Name,
//                    PostId = bookModel.PostId,
//                    Publisher = bookModel.Publisher,
//                    Title = bookModel.Title
//                });
//            }

//            _bookSearch.CreateSimilarWordsIndex();

//            return Content(User.Identity.Name);
//        }
//    }
//}
