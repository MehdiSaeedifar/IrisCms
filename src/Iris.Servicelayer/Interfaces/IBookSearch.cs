using System.Collections.Generic;
using System.Linq;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iris.Servicelayer.Interfaces
{
    public interface IBookSearch
    {
        IEnumerable<LuceneBookModel> Search(string input);

        IEnumerable<AutoCompleteSearchBookModel> AutoCompleteSearch(string input);

        IList<MorePostsLikeThis> GetMoreLikeThisPostItems(int postId);
    }

    public class BookSearch : IBookSearch
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookSearch(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<LuceneBookModel> Search(string input)
        {

            var results = _unitOfWork.Set<LuceneBookModel>()
                .FromSqlRaw(@"
SELECT TOP 50 PostId, Title, Name, Publisher, ISBN, Author, Description, SUM([Rank]) [Rank] FROM

(SELECT * FROM 
(
SELECT Top(15)
p.Id PostId,
p.Title,
p.CreatedDate,
b.Name,
b.Author,
b.Publisher,
b.ISBN,
b.Description,
pkt.[Rank]
FROM Posts p
INNER JOIN Books b ON b.Id = p.Id
INNER JOIN FREETEXTTABLE(Posts, (Title, Body), {0}) AS pkt
ON p.Id = pkt.[Key]
 ORDER BY pkt.[Rank] DESC
) posts
UNION
SELECT * FROM
(
SELECT Top(15)
p.Id PostId,
p.Title,
p.CreatedDate,
b.Name,
b.Author,
b.Publisher,
b.ISBN,
b.Description,
bkt.[Rank]
FROM Books b
INNER JOIN Posts p ON b.Id = p.Id
INNER JOIN FREETEXTTABLE
  (Books, (Name, Description, ISBN, Author, Publisher), {0}) AS bkt ON b.Id = bkt.[Key]
ORDER BY  bkt.[Rank] DESC
) books) unionPosts

GROUP BY Title, Name, PostId, CreatedDate, Author, ISBN, Publisher, Description
ORDER BY [Rank] DESC
", input)
                .AsNoTracking()
                .ToList();

            return results;
        }

        public IEnumerable<AutoCompleteSearchBookModel> AutoCompleteSearch(string input)
        {
            var results = _unitOfWork.Set<AutoCompleteSearchBookModel>()
                .FromSqlRaw(@"
SELECT TOP 20 PostId, Title, Name, SUM([Rank]) [Rank] FROM

(SELECT * FROM 
(
SELECT Top(15)
p.Id PostId,
p.Title,
p.CreatedDate,
b.Name,
pkt.[Rank]
FROM Posts p
INNER JOIN Books b ON b.Id = p.Id
INNER JOIN FREETEXTTABLE(Posts, (Title, Body), {0}) AS pkt
ON p.Id = pkt.[Key]
 ORDER BY pkt.[Rank] DESC
) posts
UNION
SELECT * FROM
(
SELECT Top(15)
p.Id PostId,
p.Title,
p.CreatedDate,
b.Name,
bkt.[Rank]
FROM Books b
INNER JOIN Posts p ON b.Id = p.Id
INNER JOIN FREETEXTTABLE
  (Books, (Name, Description, ISBN, Author, Publisher), {0}) AS bkt ON b.Id = bkt.[Key]
ORDER BY  bkt.[Rank] DESC
) books) unionPosts

GROUP BY Title, Name, PostId, CreatedDate
ORDER BY [Rank] DESC
", input)
                .AsNoTracking()
                .ToList();

            return results;
        }

        public IList<MorePostsLikeThis> GetMoreLikeThisPostItems(int postId)
        {
            return _unitOfWork.Set<MorePostsLikeThis>()
                .FromSqlRaw(@"
SELECT TOP 15 PostId, Title, SUM(score) score FROM

(SELECT * FROM 
(
SELECT Top(15)
p.Id PostId,
p.Title,
p.CreatedDate,
postSST.score
FROM SEMANTICSIMILARITYTABLE(Posts, (Title), {0}) AS postSST
INNER JOIN Posts p ON p.Id = postSST.matched_document_key
INNER JOIN Books b ON b.Id = p.Id
 ORDER BY score DESC
) posts
UNION
SELECT * FROM
(
SELECT Top(15)
p.Id PostId,
p.Title,
p.CreatedDate,
bookSST.score AS score
FROM SEMANTICSIMILARITYTABLE
  (Books, (Name, Description, Author, Publisher), {0}) AS bookSST
  INNER JOIN Books b ON b.Id = bookSST.matched_document_key
  INNER JOIN Posts p ON p.Id = b.Id
ORDER BY score DESC
) books) unionPosts

GROUP BY Title, PostId, CreatedDate
ORDER BY score DESC


", postId)
                .AsNoTracking()
                .ToList();
        }
    }
}
