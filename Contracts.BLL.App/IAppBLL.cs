using Contracts.BLL.App.Services;
using Contracts.BLL.Base;

namespace Contracts.BLL.App;

public interface IAppBLL: IBaseBLL
{
    IPilotService Pilot { get; }

}