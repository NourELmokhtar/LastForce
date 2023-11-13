using AutoMapper;
using Forces.Application.Responses.Identity;
using Forces.Infrastructure.Models.Identity;

namespace Forces.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, AppRole>().ReverseMap();
        }
    }
}