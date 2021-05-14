namespace Iris.DomainClasses.Entities
{
    public class LuceneBookModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
    }
}
