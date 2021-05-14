namespace Iris.DomainClasses.Entities
{
    public class LabelPost
    {
        public int LabelId { get; set; }

        public virtual Label Label { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}
