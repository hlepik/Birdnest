using AutoMapper;
using Contracts.BLL.Base.Mappers;
using DAL.App.DTO;

namespace BLL.App.Mappers;

public class PilotMapper : BaseMapper<BLL.App.DTO.Pilot, Pilot>

{
    public PilotMapper(IMapper mapper) : base(mapper)
    {
    }

}