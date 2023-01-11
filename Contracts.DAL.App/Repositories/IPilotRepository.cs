using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories;

public interface IPilotRepository : IBaseRepository<Pilot>, IPilotRepositoryCustom<Pilot>
{
}

public interface IPilotRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>?> FindPilots(bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllPilotsAsync(bool noTracking = true);
    Task<IEnumerable<TEntity>> PilotsToRemove(bool noTracking = true);
}