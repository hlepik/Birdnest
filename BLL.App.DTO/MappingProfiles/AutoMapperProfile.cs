using AutoMapper;

namespace BLL.App.DTO.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Pilot, DAL.App.DTO.Pilot>().ReverseMap();

    }
}