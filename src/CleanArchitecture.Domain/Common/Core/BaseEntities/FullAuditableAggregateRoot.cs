using CleanArchitecture.Domain.Common.Core.Auditing;

namespace CleanArchitecture.Domain.Common.Core.BaseEntities;

public abstract class FullAuditableAggregateRoot : AuditableAggregateRoot, IFullAuditableEntity
{
    public string DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
}

public abstract class FullAuditableAggregateRoot<TKey> : AuditableAggregateRoot<TKey>, IFullAuditableEntity<TKey>
{
    public string CreatedBy { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedTime { get; set; }
    public string DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
}