using AutoMapper;
using Forces.Application.Features.HQDepartment.Commands.AddEdit;
using Forces.Application.Features.HQDepartment.Queries.GetAll;
using Forces.Application.Features.HQDepartment.Queries.GetByForceId;

namespace Forces.Application.Mappings
{
    public class HQDepartmentsProfile : Profile
    {
        public HQDepartmentsProfile()
        {
            CreateMap<AddEditHQCommand, Models.HQDepartment>().ReverseMap();
            CreateMap<GetAllHQDepartmentsResponse, Models.HQDepartment>().ReverseMap();
            CreateMap<GetAllHQbyForceIdResponse, Models.HQDepartment>().ReverseMap();
        }
    }
}
