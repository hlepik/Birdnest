using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Repositories;
using DAL.Base.EF;

namespace DAL.App.EF;

public class AppUnitOfWork : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
{
    protected IMapper Mapper;

    public AppUnitOfWork(AppDbContext uowDbContext, IMapper mapper) : base(uowDbContext)
    {
        Mapper = mapper;
    }
    public IPilotRepository Pilot =>
        GetRepository(() => new PilotRepository(UowDbContext, Mapper));

}