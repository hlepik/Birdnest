using Contracts.BLL.Base.Services;
using Contracts.DAL.Base.Mappers;
using Contracts.Domain.Base;

namespace BLL.Base.Services;

public class BaseEntityService<TUnitOfWork, TRepository, TBllEntity, TDalentity>
    : BaseEntityService<TUnitOfWork, TRepository, TBllEntity, TDalentity, string>,
        IBaseEntityService<TBllEntity, TDalentity>
    where TBllEntity : class, IDomainEntityId
    where TDalentity : class, IDomainEntityId
    where TUnitOfWork : IBaseUnitOfWork
    where TRepository : IBaseRepository<TDalentity>
{
    public BaseEntityService(TUnitOfWork serviceUow, TRepository serviceRepository,
        IBaseMapper<TBllEntity, TDalentity> mapper) : base(serviceUow, serviceRepository, mapper)
    {
    }
}

public class
    BaseEntityService<TUnitOfWork, TRepository, TBllEntity, TDalentity, TKey> : IBaseEntityService<TBllEntity,
        TDalentity, TKey>
    where TBllEntity : class, IDomainEntityId<TKey>
    where TDalentity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TUnitOfWork : IBaseUnitOfWork
    where TRepository : IBaseRepository<TDalentity, TKey>
{
    private readonly Dictionary<TBllEntity, TDalentity> _entityCache = new();
    protected IBaseMapper<TBllEntity, TDalentity> Mapper;
    protected TRepository ServiceRepository;
    protected TUnitOfWork ServiceUow;

    public BaseEntityService(TUnitOfWork serviceUow, TRepository serviceRepository,
        IBaseMapper<TBllEntity, TDalentity> mapper)
    {
        ServiceUow = serviceUow;
        ServiceRepository = serviceRepository;
        Mapper = mapper;
    }

    public TBllEntity Add(TBllEntity entity)
    {
        var dalEntity = Mapper.Map(entity)!;
        var updatedDalEntity = ServiceRepository.Add(dalEntity);
        var returnBllEntity = Mapper.Map(updatedDalEntity)!;

        _entityCache.Add(entity, dalEntity);

        return returnBllEntity;
    }

    public TBllEntity GetUpdatedEntityAfterSaveChanges(TBllEntity entity)
    {
        var dalEntity = _entityCache[entity]!;
        var updatedDalEntity = ServiceRepository.GetUpdatedEntityAfterSaveChanges(dalEntity);
        var bllEntity = Mapper.Map(updatedDalEntity)!;
        return bllEntity;
    }

    public TBllEntity Update(TBllEntity entity)
    {
        return Mapper.Map(ServiceRepository.Update(Mapper.Map(entity)!))!;
    }

    public TBllEntity Remove(TBllEntity entity)
    {
        return Mapper.Map(ServiceRepository.Remove(Mapper.Map(entity)!))!;
    }


    public async Task<IEnumerable<TBllEntity>> GetAllAsync( bool noTracking = true)
    {
        return (await ServiceRepository.GetAllAsync(noTracking)).Select(entity => Mapper.Map(entity))!;
    }

    public async Task<TBllEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        return Mapper.Map(await ServiceRepository.FirstOrDefaultAsync(id, noTracking));
    }


    public async Task<TBllEntity> RemoveAsync(TKey id)
    {
        return Mapper.Map(await ServiceRepository.RemoveAsync(id))!;
    }
}