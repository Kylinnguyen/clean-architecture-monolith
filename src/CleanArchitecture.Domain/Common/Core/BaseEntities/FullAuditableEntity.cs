namespace CleanArchitecture.Domain.Common.Core.BaseEntities;

public abstract class FullAuditableEntity : AuditableEntity, IFullAuditableEntity
{
    public string DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
}

public abstract class FullAuditableEntity<TKey> : FullAuditableEntity, IFullAuditableEntity<TKey>
{
    public TKey Id { get; set; }
    
    public override object?[] GetKeys()
    {
        return [Id];
    }
}