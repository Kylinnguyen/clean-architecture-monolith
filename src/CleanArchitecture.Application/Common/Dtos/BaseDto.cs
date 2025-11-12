namespace CleanArchitecture.Application.Common.Dtos;

public interface IBaseCreateDto<out TEntity>
{
    public TEntity MapToEntity();
}

public interface IBaseUpdateDto<TEntity, TKey>
{
    public TKey Id { get; set; }
    public TEntity MapToEntity(TEntity entity);
}

public interface IBaseGetListDto<in TEntity>
{
    public void MapFromEntity(TEntity entity);
}

public interface IBaseGetDto<in TEntity>
{
    public void MapFromEntity(TEntity entity);
}