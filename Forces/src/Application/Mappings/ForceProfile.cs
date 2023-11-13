using AutoMapper;
using Forces.Application.Features.Forces.Commands.AddEdit;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetById;

namespace Forces.Application.Mappings
{
    public class ForceProfile : Profile
    {
        public ForceProfile()
        {
            CreateMap<AddEditForceCommand, Forces.Application.Models.Forces>().ReverseMap();
            CreateMap<GetAllForcesResponse, Forces.Application.Models.Forces>().ReverseMap();
            CreateMap<GetForceByIdResponse, Forces.Application.Models.Forces>().ReverseMap();
        }
    }
}
