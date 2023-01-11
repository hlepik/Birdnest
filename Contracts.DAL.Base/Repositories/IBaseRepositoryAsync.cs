using Contracts.DAL.Base.Repositories;
using Contracts.Domain.Base;

public interface IBaseRepositoryAsync<TEntity, TKey> : IBaseRepositoryCommon<TEntity, TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = true);
    Task<TEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true);
    Task<TEntity> RemoveAsync(TKey id);
}