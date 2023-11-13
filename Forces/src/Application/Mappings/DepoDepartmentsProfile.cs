using AutoMapper;
using Forces.Application.Features.DepoDepartment.Commands.AddEdit;
using Forces.Application.Features.DepoDepartment.Queries.GetAll;
using Forces.Application.Features.DepoDepartment.Queries.GetByForceId;

namespace Forces.Application.Mappings
{
    public class DepoDepartmentsProfile : Profile
    {
        public DepoDepartmentsProfile()
        {
            CreateMap<AddEditDepoCommand, Models.DepoDepartment>().ReverseMap();
            CreateMap<GetAllDepoDepartmentsResponse, Models.DepoDepartment>().ReverseMap();
            CreateMap<GetAllDepoByForceIdResponse, Models.DepoDepartment>().ReverseMap();
        }
    }
}
