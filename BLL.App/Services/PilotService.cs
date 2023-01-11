using AutoMapper;
using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App.Repositories;


namespace BLL.App.Services;

public class PilotService :
    BaseEntityService<IAppUnitOfWork, IPilotRepository, Pilot, DAL.App.DTO.Pilot>, IPilotService
{
    public PilotService(IAppUnitOfWork serviceUow, IPilotRepository serviceRepository, IMapper mapper) : base(
        serviceUow, serviceRepository, new PilotMapper(mapper))
    {
    }
    public async Task<IEnumerable<Pilot>?> FindPilots(bool noTracking = true)
    {
        return (await ServiceRepository.FindPilots(noTracking))!.Select(x => Mapper.Map(x))!;

    }

    public async Task<IEnumerable<Pilot>> GetAllPilotsAsync(bool noTracking = true)
    {
        return (await ServiceRepository.GetAllPilotsAsync(noTracking)).Select(x => Mapper.Map(x))!;
    }

    public async Task<IEnumerable<Pilot>> PilotsToRemove(bool noTracking = true)
    {
        return (await ServiceRepository.PilotsToRemove(noTracking)).Select(x => Mapper.Map(x))!;
    }
}