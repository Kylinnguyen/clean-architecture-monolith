using CleanArchitecture.Domain.Common.Core.Auditing;

namespace CleanArchitecture.Domain.Common.Core.BaseEntities;

public interface IFullAuditableEntity: IEntity, IFullAuditable
{

}


public interface IFullAuditableEntity<TKey> : IEntity<TKey>, IFullAuditableEntity
{
    
}