using AutoMapper;
using Forces.Application.Responses.Identity;
using Forces.Infrastructure.Models.Identity;

namespace Forces.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, Appuser>().ReverseMap()
                 .ForMember(dest => dest.DepartType, source => source.MapFrom(source => source.DepartmentType))
                 .ForMember(dest => dest.DepartId, source => source.MapFrom(source => source.DepartmentType == Application.Enums.DepartType.Depot ? source.DepoDepartmentID : source.DepartmentType == Application.Enums.DepartType.HQ ? source.HQDepartmentID : null));
            CreateMap<ChatUserResponse, Appuser>().ReverseMap()
                .ForMember(dest => dest.EmailAddress, source => source.MapFrom(source => source.Email)); //Specific Mapping
        }
    }
}