using AutoMapper;

namespace DAL.App.DTO.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Pilot, Domain.App.Pilot>().ReverseMap();


    }
}