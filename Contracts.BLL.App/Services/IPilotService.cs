using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using BLLAppDTO = BLL.App.DTO;

namespace Contracts.BLL.App.Services;

public interface IPilotService : IBaseEntityService<BLLAppDTO.Pilot, global::DAL.App.DTO.Pilot>,
    IPilotRepositoryCustom<BLLAppDTO.Pilot>
{
}