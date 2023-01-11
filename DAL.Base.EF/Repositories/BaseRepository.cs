using Contracts.DAL.Base.Mappers;
using Contracts.DAL.Base.Repositories;
using Contracts.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base.EF.Repositories;

public class BaseRepository<TDalEntity, TDomainEntity, TDbContext> :
    BaseRepository<TDalEntity, TDomainEntity, string, TDbContext>,
    IBaseRepository<TDalEntity>
    where TDalEntity : class, IDomainEntityId
    where TDomainEntity : class, IDomainEntityId
    where TDbContext : DbContext
{
    public BaseRepository(TDbContext dbContext, IBaseMapper<TDalEntity, TDomainEntity> mapper) : base(dbContext,
        mapper)
    {
    }
}

public class
    BaseRepository<TDalEntity, TDomainEntity, TKey, TDbContext> : IBaseRepository<TDalEntity, TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TDomainEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext
{
    private readonly Dictionary<TDalEntity, TDomainEntity> _entityCache = new();
    protected readonly IBaseMapper<TDalEntity, TDomainEntity> Mapper;
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TDomainEntity> RepoDbSet;

    public BaseRepository(TDbContext dbContext, IBaseMapper<TDalEntity, TDomainEntity> mapper)
    {
        RepoDbContext = dbContext;
        RepoDbSet = dbContext.Set<TDomainEntity>();
        Mapper = mapper;
    }

    public virtual async Task<IEnumerable<TDalEntity>> GetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));
        var res = await resQuery.ToListAsync();

        return res!;
    }

    public virtual async Task<TDalEntity?> FirstOrDefaultAsync(TKey id,
        bool noTracking = true)
    {
        var query = CreateQuery( noTracking);
        return Mapper.Map(await query.FirstOrDefaultAsync(e => e!.Id.Equals(id)));
    }

    public virtual TDalEntity Add(TDalEntity entity)
    {
        var domainEntity = Mapper.Map(entity)!;
        var updatedDomainEntity = RepoDbSet.Add(domainEntity).Entity;
        var dalEntity = Mapper.Map(updatedDomainEntity)!;

        _entityCache.Add(entity, domainEntity);

        return dalEntity;
    }


    public TDalEntity GetUpdatedEntityAfterSaveChanges(TDalEntity entity)
    {
        var updatedEntity = _entityCache[entity]!;
        var dalEntity = Mapper.Map(updatedEntity)!;

        return dalEntity;
    }


    public virtual TDalEntity Update(TDalEntity entity)
    {
        var domainEntity = Mapper.Map(entity);
        var updatedEntity = RepoDbSet.Update(domainEntity!).Entity;
        var dalEntity = Mapper.Map(updatedEntity);
        return dalEntity!;
    }

    public virtual TDalEntity Remove(TDalEntity entity)
    {

        return Mapper.Map(RepoDbSet.Remove(Mapper.Map(entity)!).Entity)!;
    }


    public virtual async Task<TDalEntity> RemoveAsync(TKey id)
    {
        var entity = await FirstOrDefaultAsync(id);
        if (entity == null)
            throw new NullReferenceException($" {id} not found.");
        return Remove(entity!);
    }


    protected IQueryable<TDomainEntity> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();

        if (noTracking) query = query.AsNoTracking();

        return query;
    }
}