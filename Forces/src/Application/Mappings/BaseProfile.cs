using AutoMapper;
using Forces.Application.Features.Bases.Commands.AddEdit;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Application.Features.Bases.Queries.GetByForceId;
using Forces.Application.Features.Bases.Queries.GetById;

namespace Forces.Application.Mappings
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<AddEditBaseCommand, Models.Bases>().ReverseMap();
            CreateMap<GetAllBasesResponse, Models.Bases>().ReverseMap();
            CreateMap<GetBaseByIdResponse, Models.Bases>().ReverseMap();
            CreateMap<GetAllBasesByForceIdResponse, Models.Bases>().ReverseMap();
        }
    }
}
