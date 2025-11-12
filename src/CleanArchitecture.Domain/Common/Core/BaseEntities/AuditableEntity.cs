using CleanArchitecture.Domain.Common.Core.Auditing;

namespace CleanArchitecture.Domain.Common.Core.BaseEntities;

public abstract class AuditableEntity : IAuditableEntity
{
    public string CreatedBy { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedTime { get; set; }
    public abstract object?[] GetKeys();
}

public abstract class AuditableEntity<TKey> : AuditableEntity, IAuditableEntity<TKey>
{
    public TKey Id { get; set; }
    
    public string CreatedBy { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedTime { get; set; }
}