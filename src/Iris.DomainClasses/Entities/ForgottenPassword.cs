using System;

namespace Iris.DomainClasses.Entities
{
    public class ForgottenPassword
    {
        public virtual int Id { get; set; }

        public virtual string Key { get; set; }

        public virtual DateTime ResetDateTime { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
