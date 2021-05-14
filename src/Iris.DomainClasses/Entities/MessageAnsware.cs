using System;

namespace Iris.DomainClasses.Entities
{
    public class MessageAnsware
    {
        public virtual int Id { get; set; }

        public virtual string Body { get; set; }

        public virtual DateTime AnswaredDate { get; set; }

        public int? MessageId { get; set; }

        public virtual Message Message { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
