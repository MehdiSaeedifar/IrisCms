using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using Iris.Model.LuceneModel;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Similar;
using Lucene.Net.Store;
using SpellChecker.Net.Search.Spell;
using Directory = System.IO.Directory;
using Version = Lucene.Net.Util.Version;

namespace Iris.Web.Searching
{
    public class LuceneBookSearch
    {
        private const Version _version = Version.LUCENE_30;

        private static readonly string _luceneDir =
            HttpContext.Current.Server.MapPath("~/App_Data/Lucene_Index");

        private static FSDirectory _directoryTemp;

        private static FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null)
                    _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                if (IndexWriter.IsLocked(_directoryTemp))
                    IndexWriter.Unlock(_directoryTemp);
                string lockFilePath = Path.Combine(_luceneDir, "write.lock");
                if (File.Exists(lockFilePath))
                    File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }

        private static void _addToLuceneIndex(LuceneBookModel bookData, IndexWriter writer)
        {
            // remove older index entry
            var searchQuery = new TermQuery(new Term("PostId", bookData.PostId.ToString(CultureInfo.InvariantCulture)));
            writer.DeleteDocuments(searchQuery);

            // add new index entry
            var bookDocument = new Document();

            // add lucene fields mapped to db fields
            bookDocument.Add(new Field("PostId",
                bookData.PostId.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));

            if (bookData.Author != null)
            {
                bookDocument.Add(new Field("Author", bookData.Author, Field.Store.YES, Field.Index.ANALYZED,
                    Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
            if (bookData.Title != null)
            {
                bookDocument.Add(new Field("Title", bookData.Title, Field.Store.YES, Field.Index.ANALYZED,
                    Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
            if (bookData.Name != null)
            {
                bookDocument.Add(new Field("Name", bookData.Name, Field.Store.YES, Field.Index.ANALYZED,
                    Field.TermVector.WITH_POSITIONS_OFFSETS) { Boost = 3 });
            }
            if (bookData.Publisher != null)
            {
                bookDocument.Add(new Field("Publisher", bookData.Publisher, Field.Store.YES, Field.Index.ANALYZED,
                    Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
            if (bookData.ISBN != null)
            {
                bookDocument.Add(new Field("ISBN", bookData.ISBN, Field.Store.YES, Field.Index.ANALYZED,
                    Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
            if (bookData.Description != null)
            {
                bookDocument.Add(new Field("Description", bookData.Description, Field.Store.YES, Field.Index.ANALYZED,
                    Field.TermVector.WITH_POSITIONS_OFFSETS));
            }

            // add entry to index
            writer.AddDocument(bookDocument);
        }

        public static void AddUpdateLuceneIndex(IEnumerable<LuceneBookModel> bookData)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(_version);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entry if any)
                foreach (LuceneBookModel data in bookData)
                    _addToLuceneIndex(data, writer);

                // close handles
                analyzer.Close();
                writer.Optimize();
                writer.Commit();
                writer.Dispose();
            }
        }

        public static void AddUpdateLuceneIndex(LuceneBookModel bookData)
        {
            AddUpdateLuceneIndex(new List<LuceneBookModel> { bookData });
        }

        public static void ClearLuceneIndexRecord(int recordId)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(_version);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term("PostId", recordId.ToString(CultureInfo.InvariantCulture)));
                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public static bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entries
                    writer.DeleteAll();

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static void Optimize()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }

        private static LuceneBookModel _mapLuceneDocumentToData(Document doc)
        {
            return new LuceneBookModel
            {
                PostId = Convert.ToInt32(doc.Get("PostId")),
                Name = doc.Get("Name"),
                Author = doc.Get("Author"),
                Publisher = doc.Get("Publisher"),
                ISBN = doc.Get("ISBN"),
                Description = doc.Get("Description"),
                Title = doc.Get("Title")
            };
        }

        private static IEnumerable<LuceneBookModel> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData).ToList();
        }

        private static IEnumerable<LuceneBookModel> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        private static Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        private static IEnumerable<LuceneBookModel> _search(string searchQuery, string[] searchFields)
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
                return new List<LuceneBookModel>();

            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                const int hitsLimit = 1000;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);


                var parser = new MultiFieldQueryParser
                    (Version.LUCENE_30, searchFields, analyzer);
                Query query = parseQuery(searchQuery, parser);
                ScoreDoc[] hits = searcher.Search(query, null, hitsLimit, Sort.RELEVANCE).ScoreDocs;

                if (hits.Length == 0)
                {
                    searchQuery = searchByPartialWords(searchQuery);
                    query = parseQuery(searchQuery, parser);
                    hits = searcher.Search(query, hitsLimit).ScoreDocs;
                }

                IEnumerable<LuceneBookModel> results = _mapLuceneToDataList(hits, searcher);
                analyzer.Close();
                searcher.Dispose();
                return results;
            }
        }

        public static IEnumerable<LuceneBookModel> Search(string input, params string[] fieldsName)
        {
            if (string.IsNullOrEmpty(input))
                return new List<LuceneBookModel>();

            IEnumerable<string> terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);
            return _search(input, fieldsName);
        }

        public static IEnumerable<LuceneBookModel> SearchDefault(string input, string[] fieldsName)
        {
            return string.IsNullOrEmpty(input) ? new List<LuceneBookModel>() : _search(input, fieldsName);
        }

        public static IEnumerable<LuceneBookModel> GetAllIndexRecords()
        {
            // validate search index
            if (!Directory.EnumerateFiles(_luceneDir).Any())
                return new List<LuceneBookModel>();

            // set up lucene searcher
            var searcher = new IndexSearcher(_directory, false);
            IndexReader reader = IndexReader.Open(_directory, false);
            var docs = new List<Document>();
            TermDocs term = reader.TermDocs();
            while (term.Next()) docs.Add(searcher.Doc(term.Doc));
            reader.Dispose();
            searcher.Dispose();
            return _mapLuceneToDataList(docs);
        }

        private static string searchByPartialWords(string bodyTerm)
        {
            bodyTerm = bodyTerm.Replace("*", "").Replace("?", "");
            IEnumerable<string> terms = bodyTerm.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.Trim() + "*");
            bodyTerm = string.Join(" ", terms);
            return bodyTerm;
        }

        public static string[] SuggestSilmilarWords(string term, int count = 10)
        {
            IndexReader indexReader = IndexReader.Open(FSDirectory.Open(_luceneDir), true);

            // Create the SpellChecker
            var spellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(_luceneDir + "\\Spell"));

            // Create SpellChecker Index
            spellChecker.ClearIndex();
            spellChecker.IndexDictionary(new LuceneDictionary(indexReader, "Name"));
            spellChecker.IndexDictionary(new LuceneDictionary(indexReader, "Author"));
            spellChecker.IndexDictionary(new LuceneDictionary(indexReader, "Publisher"));
            spellChecker.IndexDictionary(new LuceneDictionary(indexReader, "ISBN"));
            spellChecker.IndexDictionary(new LuceneDictionary(indexReader, "Description"));

            //Suggest Similar Words
            return spellChecker.SuggestSimilar(term, count, null, null, true);
        }

        private static int GetLuceneDocumentNumber(int postId)
        {
            var analyzer = new StandardAnalyzer(_version);
            var parser = new QueryParser(_version, "PostId", analyzer);
            Query query = parser.Parse(postId.ToString(CultureInfo.InvariantCulture));
            using (var searcher = new IndexSearcher(_directory, false))
            {
                TopDocs doc = searcher.Search(query, 1);

                return doc.TotalHits == 0 ? 0 : doc.ScoreDocs[0].Doc;
            }
        }

        private static Query CreateMoreLikeThisQuery(int postId)
        {
            int docNum = GetLuceneDocumentNumber(postId);
            if (docNum == 0)
                return null;
            var analyzer = new StandardAnalyzer(_version);
            using (var searcher = new IndexSearcher(_directory, false))
            {
                IndexReader reader = searcher.IndexReader;
                var moreLikeThis = new MoreLikeThis(reader) { Analyzer = analyzer };
                moreLikeThis.SetFieldNames(new[] { "Title", "Name", "Description", "Publisher", "Author" });
                moreLikeThis.MinDocFreq = 1;
                moreLikeThis.MinTermFreq = 1;
                moreLikeThis.Boost = true;
                return moreLikeThis.Like(docNum);
            }
        }

        public static IList<MorePostsLikeThis> GetMoreLikeThisPostItems(int postId)
        {
            Query query = CreateMoreLikeThisQuery(postId);
            if (query == null)
                return new List<MorePostsLikeThis>();
            using (var searcher = new IndexSearcher(_directory, false))
            {
                TopDocs hits = searcher.Search(query, 10);
                return hits.ScoreDocs.Select(item => searcher.Doc(item.Doc)).Select(doc => new MorePostsLikeThis
                {
                    Title = doc.Get("Title"),
                    PostId = doc.Get("PostId")
                }).ToList();
            }
        }
    }
}