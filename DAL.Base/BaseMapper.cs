using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.Base;

public class BaseMapper<TLeftObject, TRightObject> : IBaseMapper<TLeftObject, TRightObject>
    where TRightObject : class?, new()
    where TLeftObject : class?, new()
{
    protected readonly MapperConfigurationExpression MapperConfigurationExpression;


    protected IMapper Mapper;

    public BaseMapper()
    {
        MapperConfigurationExpression = new MapperConfigurationExpression();
        MapperConfigurationExpression.CreateMap<TLeftObject, TRightObject>();
        MapperConfigurationExpression.CreateMap<TRightObject, TLeftObject>();

        Mapper = new Mapper(new MapperConfiguration(MapperConfigurationExpression));
    }

    public virtual TRightObject Map(TLeftObject? inObject)
    {
        return Mapper.Map<TLeftObject, TRightObject>(inObject!);
    }

    public virtual TLeftObject Map(TRightObject? inObject)
    {
        return Mapper.Map<TRightObject, TLeftObject>(inObject!);
    }
}