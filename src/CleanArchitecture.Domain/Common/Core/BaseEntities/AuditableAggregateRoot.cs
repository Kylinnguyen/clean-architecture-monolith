using CleanArchitecture.Domain.Common.Core.Auditing;

namespace CleanArchitecture.Domain.Common.Core.BaseEntities;

public abstract class AuditableAggregateRoot : AggregateRoot, IAuditableEntity
{
    public string CreatedBy { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedTime { get; set; }
}

public abstract class AuditableAggregateRoot<TKey> : AggregateRoot<TKey>, IAuditableEntity<TKey>
{
    public string CreatedBy { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedTime { get; set; }
}