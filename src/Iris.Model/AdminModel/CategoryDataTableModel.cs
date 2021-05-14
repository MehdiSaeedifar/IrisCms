namespace Iris.Model.AdminModel
{
    public class CategoryDataTableModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ArticlesCount { get; set; }
        public int? Order { get; set; }
    }
}