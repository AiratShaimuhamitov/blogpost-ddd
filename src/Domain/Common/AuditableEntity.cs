using System;
using Blogpost.Domain.Entities;

namespace Blogpost.Domain.Common
{
    public interface IAuditableEntity
    {
        DateTime Created { get; set; }

        Guid CreatedById { get; set; }
        Profile CreatedBy { get; set; }

        DateTime? LastModified { get; set; }

        Guid? LastModifiedById { get; set; }
        Profile LastModifiedBy { get; set; }
    }


    public abstract class AuditableEntity<TKey> : Entity<TKey>, IAuditableEntity
        where TKey : struct
    {
        public DateTime Created { get; set; }

        public Guid CreatedById { get; set; }
        public virtual Profile CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public Guid? LastModifiedById { get; set; }
        public virtual Profile LastModifiedBy { get; set; }
    }
}