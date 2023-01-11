using Contracts.DAL.Base.Repositories;
using Contracts.Domain.Base;

public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, string>
    where TEntity : class, IDomainEntityId<string>
{
}

public interface IBaseRepository<TEntity, TKey> : IBaseRepositoryAsync<TEntity, TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
}