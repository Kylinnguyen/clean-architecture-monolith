using CleanArchitecture.Domain.Common.Core.BaseEntities;

namespace CleanArchitecture.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : class, IEntity;
