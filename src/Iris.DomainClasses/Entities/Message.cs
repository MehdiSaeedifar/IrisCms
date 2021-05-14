using System;
using System.Collections.Generic;

namespace Iris.DomainClasses.Entities
{
    public class Message
    {
        public virtual int Id { get; set; }

        public virtual string Body { get; set; }

        public virtual bool IsAnswared { get; set; }

        public virtual string Subject { get; set; }

        public virtual DateTime AddedDate { get; set; }

        public virtual ICollection<MessageAnsware> Answares { get; set; }

        public int? UserId { get; set; }

        public User User { get; set; }
    }
}
