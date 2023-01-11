using Contracts.DAL.Base.Mappers;
using Domain.App;

namespace DAL.App.EF.Mappers;

public class PilotMapper : BaseMapper<DAL.App.DTO.Pilot, Pilot>
{
    public PilotMapper(IMapper mapper) : base(mapper)
    {
    }
}