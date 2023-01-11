using AutoMapper;
using BLL.App.Services;
using Contracts.BLL.App;
using Contracts.BLL.App.Services;

namespace BLL.App;

public class AppBLL: BaseBLL<IAppUnitOfWork>, IAppBLL
{
    protected IMapper Mapper;

    public AppBLL(IAppUnitOfWork uow, IMapper mapper) : base(uow)
    {
        Mapper = mapper;
    }
    public IPilotService Pilot =>

        GetService<IPilotService>(() => new PilotService(Uow, Uow.Pilot, Mapper));
}