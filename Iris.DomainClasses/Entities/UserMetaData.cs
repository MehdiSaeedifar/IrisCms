using System;

namespace Iris.DomainClasses.Entities
{
    public class UserMetaData
    {
        public virtual long Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Major { get; set; }
        public virtual string Description { get; set; }
        public virtual string AvatarPath { get; set; }
        public virtual DateTime? BirthDay { get; set; }
        public virtual User User { get; set; }
    }
}